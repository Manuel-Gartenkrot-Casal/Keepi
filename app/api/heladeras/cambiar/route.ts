import { NextResponse } from "next/server"
import { getSession, setSession } from "@/lib/session"

export async function POST(request: Request) {
  try {
    const session = await getSession()

    if (!session) {
      return NextResponse.json({ error: "No autorizado" }, { status: 401 })
    }

    const { nombreHeladera } = await request.json()

    if (!nombreHeladera) {
      return NextResponse.json({ error: "Nombre de heladera requerido" }, { status: 400 })
    }

    // Update session with new fridge
    await setSession({
      ...session,
      nombreHeladera,
    })

    return NextResponse.json({ success: true, heladeraActual: nombreHeladera })
  } catch (error) {
    console.error("[Heladeras] Error cambiando heladera:", error)
    return NextResponse.json({ error: "Error al cambiar heladera" }, { status: 500 })
  }
}
