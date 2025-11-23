"use client"

import { useState, useEffect } from "react"
import { Bell, Refrigerator, AlertTriangle, Calendar, LogOut, ChevronDown } from "lucide-react"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuTrigger, DropdownMenuItem } from "@/components/ui/dropdown-menu"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { LoginForm } from "@/components/login-form"

interface ProductoHeladera {
  id: number
  nombreEspecifico: string
  nombreProducto: string
  fechaVencimiento: string
  diasRestantes: number
  foto: string
}

interface SessionData {
  authenticated: boolean
  usuario?: { id: number; username: string }
  heladeras?: string[]
  heladeraActual?: string
}

export default function Page() {
  const [session, setSession] = useState<SessionData | null>(null)
  const [productos, setProductos] = useState<ProductoHeladera[]>([])
  const [productosVenciendo, setProductosVenciendo] = useState<ProductoHeladera[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [checkingAuth, setCheckingAuth] = useState(true)

  useEffect(() => {
    async function checkAuth() {
      try {
        const response = await fetch("/api/auth/session")
        if (response.ok) {
          const data = await response.json()
          setSession(data)
        } else {
          setSession({ authenticated: false })
        }
      } catch (err) {
        console.error("[v0] Error checking auth:", err)
        setSession({ authenticated: false })
      } finally {
        setCheckingAuth(false)
      }
    }
    checkAuth()
  }, [])

  useEffect(() => {
    if (!session?.authenticated) return

    async function fetchProductos() {
      try {
        setLoading(true)
        const response = await fetch("/api/productos-vencimiento")

        if (!response.ok) {
          if (response.status === 401) {
            setSession({ authenticated: false })
            return
          }
          throw new Error("Error al cargar productos")
        }

        const data = await response.json()
        setProductos(data.todos || [])
        setProductosVenciendo(data.venciendo || [])
        setError(null)
      } catch (err) {
        console.error("[v0] Error:", err)
        setError(err instanceof Error ? err.message : "Error desconocido")
      } finally {
        setLoading(false)
      }
    }

    fetchProductos()
    const interval = setInterval(fetchProductos, 5 * 60 * 1000)
    return () => clearInterval(interval)
  }, [session])

  const handleLogout = async () => {
    await fetch("/api/auth/logout", { method: "POST" })
    setSession({ authenticated: false })
    setProductos([])
    setProductosVenciendo([])
  }

  const handleCambiarHeladera = async (nombreHeladera: string) => {
    try {
      const response = await fetch("/api/heladeras/cambiar", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ nombreHeladera }),
      })

      if (response.ok) {
        setSession((prev) => (prev ? { ...prev, heladeraActual: nombreHeladera } : null))
        window.location.reload()
      }
    } catch (err) {
      console.error("[v0] Error cambiando heladera:", err)
    }
  }

  const getExpirationColor = (diasRestantes: number) => {
    if (diasRestantes < 0) return "text-red-600"
    if (diasRestantes === 0) return "text-red-500"
    if (diasRestantes === 1) return "text-orange-500"
    if (diasRestantes <= 3) return "text-yellow-600"
    return "text-green-600"
  }

  const getExpirationMessage = (diasRestantes: number) => {
    if (diasRestantes < 0) return "Vencido"
    if (diasRestantes === 0) return "Vence hoy"
    if (diasRestantes === 1) return "Vence mañana"
    return `Vence en ${diasRestantes} días`
  }

  if (checkingAuth) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-green-50 to-blue-50 flex items-center justify-center">
        <p className="text-gray-500">Cargando...</p>
      </div>
    )
  }

  if (!session?.authenticated) {
    return <LoginForm onLoginSuccess={() => window.location.reload()} />
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-green-50 to-blue-50">
      {/* Header with Notifications */}
      <header className="bg-white border-b border-gray-200 sticky top-0 z-50 shadow-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 bg-green-500 rounded-lg flex items-center justify-center">
                <Refrigerator className="w-6 h-6 text-white" />
              </div>
              <h1 className="text-xl font-bold text-gray-900">Keepi</h1>
            </div>

            <div className="flex items-center gap-4">
              {session.heladeras && session.heladeras.length > 1 && (
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button variant="outline" size="sm" className="gap-2 bg-transparent">
                      <Refrigerator className="w-4 h-4" />
                      {session.heladeraActual}
                      <ChevronDown className="w-4 h-4" />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                    {session.heladeras.map((heladera) => (
                      <DropdownMenuItem
                        key={heladera}
                        onClick={() => handleCambiarHeladera(heladera)}
                        className={heladera === session.heladeraActual ? "bg-green-50 font-medium" : ""}
                      >
                        {heladera}
                      </DropdownMenuItem>
                    ))}
                  </DropdownMenuContent>
                </DropdownMenu>
              )}

              {/* Notifications Dropdown */}
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button variant="ghost" size="icon" className="relative">
                    <Bell className="w-5 h-5" />
                    {productosVenciendo.length > 0 && (
                      <Badge
                        variant="destructive"
                        className="absolute -top-1 -right-1 h-5 w-5 flex items-center justify-center p-0 text-xs"
                      >
                        {productosVenciendo.length}
                      </Badge>
                    )}
                    <span className="sr-only">Notificaciones</span>
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="w-80 p-0">
                  <div className="bg-white rounded-lg shadow-lg">
                    <div className="px-4 py-3 border-b border-gray-200">
                      <h3 className="font-semibold text-gray-900">Productos por vencer</h3>
                      <p className="text-sm text-gray-500">
                        {productosVenciendo.length} producto{productosVenciendo.length !== 1 ? "s" : ""} requieren
                        atención
                      </p>
                    </div>

                    <div className="max-h-96 overflow-y-auto">
                      {loading ? (
                        <div className="px-4 py-8 text-center text-gray-500">
                          <p className="text-sm">Cargando productos...</p>
                        </div>
                      ) : productosVenciendo.length > 0 ? (
                        <div className="divide-y divide-gray-100">
                          {productosVenciendo.map((producto) => (
                            <div key={producto.id} className="px-4 py-3 hover:bg-gray-50 transition-colors">
                              <div className="flex items-start gap-3">
                                <img
                                  src={producto.foto || "/placeholder.svg"}
                                  alt={producto.nombreEspecifico}
                                  className="w-12 h-12 rounded-lg object-cover flex-shrink-0"
                                />
                                <div className="flex-1 min-w-0">
                                  <p className="font-medium text-gray-900 text-sm truncate">
                                    {producto.nombreEspecifico}
                                  </p>
                                  <div className="flex items-center gap-2 mt-1">
                                    <Calendar className="w-3 h-3 text-gray-400" />
                                    <p className={`text-xs font-medium ${getExpirationColor(producto.diasRestantes)}`}>
                                      {getExpirationMessage(producto.diasRestantes)}
                                    </p>
                                  </div>
                                </div>
                                {producto.diasRestantes < 0 && (
                                  <AlertTriangle className="w-5 h-5 text-red-500 flex-shrink-0" />
                                )}
                              </div>
                            </div>
                          ))}
                        </div>
                      ) : (
                        <div className="px-4 py-8 text-center text-gray-500">
                          <Bell className="w-8 h-8 mx-auto mb-2 text-gray-300" />
                          <p className="text-sm">No hay productos por vencer</p>
                        </div>
                      )}
                    </div>
                  </div>
                </DropdownMenuContent>
              </DropdownMenu>

              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button variant="ghost" size="sm" className="gap-2">
                    {session.usuario?.username}
                    <ChevronDown className="w-4 h-4" />
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                  <DropdownMenuItem onClick={handleLogout} className="text-red-600">
                    <LogOut className="w-4 h-4 mr-2" />
                    Cerrar sesión
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {error && (
          <Alert className="mb-6 border-red-200 bg-red-50">
            <AlertTriangle className="h-4 w-4 text-red-600" />
            <AlertDescription className="text-red-800">
              Error al cargar productos: {error}. Verifica la conexión a la base de datos.
            </AlertDescription>
          </Alert>
        )}

        {!loading && productosVenciendo.length > 0 && (
          <Alert className="mb-6 border-orange-200 bg-orange-50">
            <AlertTriangle className="h-4 w-4 text-orange-600" />
            <AlertDescription className="text-orange-800">
              Tienes <strong>{productosVenciendo.length}</strong> producto{productosVenciendo.length !== 1 ? "s" : ""}{" "}
              que {productosVenciendo.length !== 1 ? "están" : "está"} por vencer o ya vencido
              {productosVenciendo.length !== 1 ? "s" : ""}. Revisa las notificaciones para más detalles.
            </AlertDescription>
          </Alert>
        )}

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Refrigerator className="w-5 h-5" />
              {session.heladeraActual}
            </CardTitle>
            <CardDescription>
              {loading ? "Cargando productos..." : `${productos.length} productos almacenados`}
            </CardDescription>
          </CardHeader>
          <CardContent>
            {loading ? (
              <div className="text-center py-8 text-gray-500">
                <p>Cargando productos desde la base de datos...</p>
              </div>
            ) : productos.length > 0 ? (
              <div className="space-y-4">
                {productos.map((producto) => (
                  <div
                    key={producto.id}
                    className="flex items-center gap-4 p-4 rounded-lg border border-gray-200 hover:border-gray-300 transition-colors"
                  >
                    <img
                      src={producto.foto || "/placeholder.svg"}
                      alt={producto.nombreEspecifico}
                      className="w-16 h-16 rounded-lg object-cover"
                    />
                    <div className="flex-1">
                      <h3 className="font-semibold text-gray-900">{producto.nombreEspecifico}</h3>
                      <p className="text-sm text-gray-500">{producto.nombreProducto}</p>
                    </div>
                    <div className="text-right">
                      <p className={`font-medium text-sm ${getExpirationColor(producto.diasRestantes)}`}>
                        {getExpirationMessage(producto.diasRestantes)}
                      </p>
                      <p className="text-xs text-gray-500 mt-1">
                        {new Date(producto.fechaVencimiento).toLocaleDateString("es-AR")}
                      </p>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <div className="text-center py-8 text-gray-500">
                <Refrigerator className="w-12 h-12 mx-auto mb-2 text-gray-300" />
                <p>No hay productos en la heladera</p>
              </div>
            )}
          </CardContent>
        </Card>
      </main>
    </div>
  )
}
