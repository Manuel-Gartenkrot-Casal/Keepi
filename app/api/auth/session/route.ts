import { NextResponse } from "next/server"
import { getSession } from "@/lib/session"
import { traerNombresHeladerasById } from "@/lib/db"

export const dynamic = "force-dynamic"

export async function GET() {
  try {
    const session = await getSession()

    if (!session) {
      return NextResponse.json({ authenticated: false }, { status: 401 })
    }

    const heladeras = await traerNombresHeladerasById(session.usuario.ID)

    return NextResponse.json({
      authenticated: true,
      usuario: {
        id: session.usuario.ID,
        username: session.usuario.Username,
      },
      heladeras,
      heladeraActual: session.nombreHeladera,
    })
  } catch (error) {
    console.error("[Auth] Error obteniendo sesión:", error)
    return NextResponse.json({ error: "Error al obtener sesión" }, { status: 500 })
  }
}
