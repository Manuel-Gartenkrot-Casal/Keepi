import sql from "mssql"

const config: sql.config = {
  server: process.env.DB_SERVER || "localhost",
  database: process.env.DB_NAME || "Keepi_DataBase",
  options: {
    encrypt: false,
    trustServerCertificate: true,
  },
  authentication: {
    type: "default",
    options: {
      userName: process.env.DB_USER,
      password: process.env.DB_PASSWORD,
    },
  },
}

// If no user/password provided, use Windows authentication
if (!process.env.DB_USER) {
  config.authentication = {
    type: "ntlm",
    options: {
      domain: "",
    },
  }
}

let pool: sql.ConnectionPool | null = null

export async function getDbConnection() {
  if (!pool) {
    pool = await sql.connect(config)
  }
  return pool
}

export interface ProductoXHeladera {
  Id: number
  IdHeladera: number
  IdProducto: number
  NombreEspecifico: string
  NombreProducto: string | null
  FechaVencimiento: Date
  Eliminado: boolean
  Abierto: boolean
  Foto: string
}

export interface Usuario {
  ID: number
  Username: string
  Password: string
}

export async function verificarUsuario(username: string, password: string): Promise<Usuario | null> {
  try {
    const pool = await getDbConnection()
    const result = await pool
      .request()
      .input("Username", sql.VarChar, username)
      .input("Password", sql.VarChar, password)
      .execute("verificarUsuario")

    if (result.recordset && result.recordset.length > 0) {
      return result.recordset[0]
    }
    return null
  } catch (error) {
    console.error("[DB] Error verificando usuario:", error)
    return null
  }
}

export async function traerNombresHeladerasById(idUsuario: number): Promise<string[]> {
  try {
    const pool = await getDbConnection()
    const result = await pool.request().input("IdUsuario", sql.Int, idUsuario).execute("traerNombresHeladerasById")

    return result.recordset.map((row: any) => row.Nombre || row.nombre)
  } catch (error) {
    console.error("[DB] Error obteniendo heladeras:", error)
    return []
  }
}

export async function getProductosByHeladeraAndUsuario(
  nombreHeladera: string,
  idUsuario: number,
): Promise<ProductoXHeladera[]> {
  const pool = await getDbConnection()
  const result = await pool
    .request()
    .input("nombre", sql.VarChar, nombreHeladera)
    .input("IdUsuario", sql.Int, idUsuario)
    .execute("getProductosByNombreHeladeraAndIdUsuario")

  return result.recordset
}
