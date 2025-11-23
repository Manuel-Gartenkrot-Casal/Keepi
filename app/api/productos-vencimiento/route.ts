import { NextResponse } from "next/server"
import { getProductosByHeladeraAndUsuario } from "@/lib/db"
import { getSession } from "@/lib/session"

export const dynamic = "force-dynamic"

interface ProductoVencimiento {
  id: number
  nombreEspecifico: string
  nombreProducto: string
  fechaVencimiento: string
  diasRestantes: number
  foto: string
}

function calcularDiasRestantes(fechaVencimiento: Date): number {
  const hoy = new Date()
  hoy.setHours(0, 0, 0, 0)
  const vencimiento = new Date(fechaVencimiento)
  vencimiento.setHours(0, 0, 0, 0)
  const diferencia = vencimiento.getTime() - hoy.getTime()
  return Math.ceil(diferencia / (1000 * 60 * 60 * 24))
}

export async function GET() {
  try {
    const session = await getSession()

    if (!session) {
      return NextResponse.json({ error: "No autorizado" }, { status: 401 })
    }

    const { usuario, nombreHeladera } = session

    console.log("[v0] Fetching productos for:", { nombreHeladera, idUsuario: usuario.ID })

    const productos = await getProductosByHeladeraAndUsuario(nombreHeladera, usuario.ID)

    console.log("[v0] Found productos:", productos.length)

    // Filtrar productos no eliminados y calcular días restantes
    const productosConDias: ProductoVencimiento[] = productos
      .filter((p) => !p.Eliminado)
      .map((p) => ({
        id: p.Id,
        nombreEspecifico: p.NombreEspecifico,
        nombreProducto: p.NombreProducto || "Producto",
        fechaVencimiento: p.FechaVencimiento.toISOString(),
        diasRestantes: calcularDiasRestantes(p.FechaVencimiento),
        foto: p.Foto || "/placeholder.svg",
      }))

    // Filtrar solo los que vencen en 3 días o menos, o ya vencidos
    const productosVenciendo = productosConDias.filter((p) => p.diasRestantes <= 3)

    console.log("[v0] Productos venciendo:", productosVenciendo.length)

    return NextResponse.json({
      todos: productosConDias,
      venciendo: productosVenciendo,
    })
  } catch (error) {
    console.error("[v0] Error fetching productos:", error)
    return NextResponse.json(
      { error: "Error al obtener productos", details: error instanceof Error ? error.message : "Unknown error" },
      { status: 500 },
    )
  }
}
