export interface Usuario {
  ID: number
  Username: string
}

export interface Heladera {
  ID: number
  Nombre: string
  Color: string
  Eliminado: boolean
}

export interface ProductoXHeladera {
  ID: number
  IdHeladera: number
  IdProducto: number
  NombreEspecifico: string
  NombreProducto: string | null
  FechaVencimiento: Date
  Eliminado: boolean
  Abierto: boolean
  Foto: string
  IdUsuario?: number
}
