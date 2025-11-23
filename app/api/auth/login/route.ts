import { NextResponse } from "next/server"
import { verificarUsuario, traerNombresHeladerasById } from "@/lib/db"
import { setSession } from "@/lib/session"

export async function POST(request: Request) {
  try {
    const { username, password } = await request.json()

    if (!username || !password) {
      return NextResponse.json({ error: "Usuario y contraseña son requeridos" }, { status: 400 })
    }

    const usuario = await verificarUsuario(username, password)

    if (!usuario) {
      return NextResponse.json({ error: "Usuario o contraseña incorrectos" }, { status: 401 })
    }

    // Get user's fridges
    const heladeras = await traerNombresHeladerasById(usuario.ID)

    if (heladeras.length === 0) {
      return NextResponse.json({ error: "Usuario no tiene heladeras configuradas" }, { status: 400 })
    }

    // Set session with first fridge as default
    await setSession({
      usuario: {
        ID: usuario.ID,
        Username: usuario.Username,
        Password: "", // Don't store password in session
      },
      nombreHeladera: heladeras[0],
    })

    return NextResponse.json({
      success: true,
      usuario: {
        id: usuario.ID,
        username: usuario.Username,
      },
      heladeras,
      heladeraActual: heladeras[0],
    })
  } catch (error) {
    console.error("[Auth] Error en login:", error)
    return NextResponse.json({ error: "Error al iniciar sesión" }, { status: 500 })
  }
}
