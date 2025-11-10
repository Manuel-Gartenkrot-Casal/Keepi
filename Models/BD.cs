    using Microsoft.Data.SqlClient;
    using System.Data;
    using Dapper;
    using Keepi.Models;
    using System.Linq;

    public class BD
    {
        private static string _connectionString =
        @"Server=localhost; Database=Keepi_DataBase; Trusted_Connection=True; TrustServerCertificate=True;";


        public static double CalcularDuracionProducto(string producto, double D0, double acidez, double agua, double azucar, double conservantes, double alcohol, double porcentajeCambio, int diasAbiertos, double fPromedioBase)
        {

            double duracionRestante = -1; //en controller se tendrían que calcular las fechas exactas
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string StoredProcedure = "TeoremaHevia";

                duracionRestante = connection.QueryFirstOrDefault<double>(
                StoredProcedure,
                new
                {
                    Producto = producto, //producto siendo el producto al que se le calculará  
                    D0 = D0, //D0 siendo los días que le quedaban inicialmente al producto  
                    Acidez = acidez, //Acidez siendo el porcentaje de acidez del product
                    Agua = agua, //porcentajeAgua  
                    Azucar = azucar, //porcentajeAzucar  
                    Conservantes = conservantes, //porcentajeConservantes
                    Alcohol = alcohol, //porcentajeAlcohol  
                    PorcentajeCambio = porcentajeCambio, //hay una variable llamada así, es lo rápido que se alterará el producto   
                    DiasAbierto = diasAbiertos, //los días que lleva abierto desde el inicio así se le resta
                    f_promedio_base = fPromedioBase //un avg entre todos los promedios conseguido en el segundo sp
                },
                commandType: CommandType.StoredProcedure
            );
            }
            return duracionRestante;
        }
        public static double calcularFPromedioBase()
        {

            double fPromedioBase = -1;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string StoredProcedure = "CalcularFPromedioBase";

                fPromedioBase = connection.QueryFirstOrDefault<double>(
                    StoredProcedure,
                    commandType: CommandType.StoredProcedure
                );
            }
            return fPromedioBase;
        }
        public static Usuario verificarUsuario(string Username, string Password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { Username = Username, Password = Password };
                Usuario user = connection.QueryFirstOrDefault<Usuario>(
                    "[dbo].[verificarUsuario]",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return user;
            }
        }

        public static Producto buscarProducto(string nombre)
        {

            Producto producto = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string StoredProcedure = "buscarProducto";

                producto = connection.QueryFirstOrDefault<Producto>(
                    StoredProcedure,
                    commandType: CommandType.StoredProcedure
                );

                return producto;
            }
        }
        public static int agregarHeladera(string nombre, string color)
        {
            int resultado = -1;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string StoredProcedure = "crearHeladera";

                resultado = connection.QueryFirstOrDefault<int>(
                    StoredProcedure,
                    commandType: CommandType.StoredProcedure
                );

                return resultado;
            }
        }
        public static int borrarHeladera(string nombre, string username)
        {
            int resultado = -1;
            //modificar agregando que tambien se tenga que tener el username adecuado antes de borrar para no borrar la heladera de otro si tiene el mismo nombre
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string StoredProcedure = "eliminarHeladera";

                resultado = connection.QueryFirstOrDefault<int>(
                    StoredProcedure,
                    commandType: CommandType.StoredProcedure
                );

                return resultado;
            }
        }
        public static int crearUsuario(string usernameIngresado, string passwordIngresada)
        {
            int sePudo = -1;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string StoredProcedure = "crearUsuario";
                sePudo = connection.Query<int>(StoredProcedure, new { Username = usernameIngresado, Password = passwordIngresada }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return sePudo;
        }
        // --- MÉTODOS PARA RECETAS (PEGAR DENTRO DE LA CLASE BD) ---

        public static List<Receta> GetAllRecetas()
        {
            List<Receta> lista = new List<Receta>();
            string storedProcedure = "sp_GetRecetas"; // Asumo este nombre para tu Stored Procedure
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                lista = connection.Query<Receta>(storedProcedure, commandType: CommandType.StoredProcedure).ToList();
            }
            return lista;
        }

        public static Receta GetRecetaById(int idReceta)
        {
            Receta receta = null;
            string storedProcedure = "sp_GetRecetaById";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                receta = connection.QueryFirstOrDefault<Receta>(
                    storedProcedure,
                    new { IdReceta = idReceta },
                    commandType: CommandType.StoredProcedure
                );
            }
            return receta;
        }

        public static List<Producto> GetProductosByRecetaId(int idReceta)
        {
            List<Producto> lista = new List<Producto>();
            string storedProcedure = "sp_GetProductosByRecetaId";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                lista = connection.Query<Producto>(
                    storedProcedure,
                    new { IdReceta = idReceta },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
            return lista;
        }

        public static void CrearReceta(Receta receta)
        {
            string storedProcedure = "sp_CrearReceta";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Asumo los campos de tu modelo Receta.cs
                connection.Execute(
                    storedProcedure,
                    new
                    {
                        nombre = receta.nombre,
                        favorito = receta.favorito,
                        duracion = receta.duracion
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static List<Producto> GetAllProductos()
        {
            List<Producto> lista = new List<Producto>();
            string storedProcedure = "sp_GetAllProductos";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                lista = connection.Query<Producto>(storedProcedure, commandType: CommandType.StoredProcedure).ToList();
            }
            return lista;
        }
        public static List<ProductoXHeladera> getProductosByNombreHeladeraAndIdUsuario(string nombreHeladera, int idUsuario)
        {
            List<ProductoXHeladera> lista = new List<ProductoXHeladera>();
            string storedProcedure = "getProductosByNombreHeladeraAndIdUsuario";
            var parameters = new { nombre = nombreHeladera, IdUsuario = idUsuario };
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Usar Query con mapeo dinámico para evitar conflictos con columnas de otras tablas en los JOINs
                var results = connection.Query(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                foreach (var row in results)
                {
                    var rowDict = (IDictionary<string, object>)row;
                    var producto = new ProductoXHeladera
                    {
                        ID = rowDict.ContainsKey("Id") && rowDict["Id"] != null ? Convert.ToInt32(rowDict["Id"]) : 0,
                        IdHeladera = rowDict.ContainsKey("IdHeladera") && rowDict["IdHeladera"] != null ? Convert.ToInt32(rowDict["IdHeladera"]) : 0,
                        IdProducto = rowDict.ContainsKey("IdProducto") && rowDict["IdProducto"] != null ? Convert.ToInt32(rowDict["IdProducto"]) : 0,
                        NombreEspecifico = rowDict.ContainsKey("NombreEspecifico") && rowDict["NombreEspecifico"] != null ? rowDict["NombreEspecifico"].ToString() : string.Empty,
                        NombreProducto = rowDict.ContainsKey("NombreProducto") && rowDict["NombreProducto"] != null ? rowDict["NombreProducto"].ToString() : null,
                        FechaVencimiento = rowDict.ContainsKey("FechaVencimiento") && rowDict["FechaVencimiento"] != null ? Convert.ToDateTime(rowDict["FechaVencimiento"]) : DateTime.MinValue,
                        Eliminado = rowDict.ContainsKey("Eliminado") && rowDict["Eliminado"] != null ? Convert.ToBoolean(rowDict["Eliminado"]) : false,
                        Abierto = rowDict.ContainsKey("Abierto") && rowDict["Abierto"] != null ? Convert.ToBoolean(rowDict["Abierto"]) : false,
                        Foto = rowDict.ContainsKey("Foto") && rowDict["Foto"] != null ? rowDict["Foto"].ToString() : string.Empty
                    };
                    // Intentar obtener IdUsuario si está en el resultado (puede venir del JOIN con UsuarioXHeladera)
                    if (rowDict.ContainsKey("IdUsuario") && rowDict["IdUsuario"] != null)
                    {
                        producto.IdUsuario = Convert.ToInt32(rowDict["IdUsuario"]);
                    }
                    lista.Add(producto);
                }
            }
            return lista;
        }

        public static void UpdateReceta(Receta receta)
        {
            string storedProcedure = "sp_UpdateReceta";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Asumo los campos de tu modelo Receta.cs
                connection.Execute(
                    storedProcedure,
                    new
                    {
                        ID = receta.ID,
                        nombre = receta.nombre,
                        favorito = receta.favorito,
                        duracion = receta.duracion
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static void DeleteProductosXRecetaByIdReceta(int idReceta)
        {
            string storedProcedure = "sp_DeleteProductosXRecetaByIdReceta";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    new { IdReceta = idReceta },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static void DeleteReceta(int idReceta)
        {
            string storedProcedure = "sp_DeleteReceta";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    new { IdReceta = idReceta },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static Heladera GetHeladeraByUsuarioId(int idUsuario)
        {
            Heladera heladera = null;
            string storedProcedure = "sp_GetHeladeraByUsuarioId";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                heladera = connection.QueryFirstOrDefault<Heladera>(
                    storedProcedure,
                    new { IdUsuario = idUsuario },
                    commandType: CommandType.StoredProcedure
                );
            }
            return heladera;
        }

        public static List<Producto> GetProductosByHeladeraId(int idHeladera)
        {
            List<Producto> lista = new List<Producto>();
            string storedProcedure = "sp_GetProductosByHeladeraId";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                lista = connection.Query<Producto>(
                    storedProcedure,
                    new { IdHeladera = idHeladera },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
            return lista;
        }

        public static void AgregarProductoAReceta(int idReceta, int idProducto)
        {
            string storedProcedure = "sp_AgregarProductoAReceta";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    new { IdReceta = idReceta, IdProducto = idProducto },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static void QuitarProductoDeReceta(int idReceta, int idProducto)
        {
            string storedProcedure = "sp_QuitarProductoDeReceta";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    new { IdReceta = idReceta, IdProducto = idProducto },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public static List<string> traerNombresHeladerasById(int idUsuario)
        {
            List<string> nombresHeladera = new List<string>();
            string storedProcedure = "traerNombresHeladerasById";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                nombresHeladera = connection.Query<string>(
                    storedProcedure,
                    new { IdUsuario = idUsuario },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }

            return nombresHeladera;
        }

        public static Heladera SeleccionarHeladeraByNombre(int idUsuario, string nombreHeladera)
        {
            Heladera heladera;
            string storedProcedure = "SeleccionarHeladeraByNombre";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                heladera = connection.QueryFirstOrDefault<Heladera>(
                    storedProcedure,
                    new { IdUsuario = idUsuario, Nombre = nombreHeladera },
                    commandType: CommandType.StoredProcedure
                );
            }
            
            return heladera;
        }
        public static void agregarProductoExistente(int idHeladera, int idProducto, string nombreEsp, DateTime fechaVencimiento, string foto)
        {
            string storedProcedure = "sp_AgregarProductoXHeladera"; 
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    new { 
                        IdHeladera = idHeladera,
                        IdProducto = idProducto,
                        NombreEsp = nombreEsp,
                        FechaVencimiento = fechaVencimiento,
                        Eliminado = 0, 
                        Abierto = 0,  
                        Foto = foto
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static List<ProductoXHeladera> GetProductosXHeladeraByHeladeraId(int idHeladera)
        {
            List<ProductoXHeladera> lista = new List<ProductoXHeladera>();
            string storedProcedure = "sp_GetProductosByHeladeraId";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                lista = connection.Query<ProductoXHeladera>(
                    storedProcedure,
                    new { IdHeladera = idHeladera },
                    commandType: CommandType.StoredProcedure
                ).ToList();
                }
            return lista;
        }

        public static void EliminarProductoXHeladera(int idHeladera, int idProducto)
        {
            string storedProcedure = "eliminarProductoHeladera";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    new { idHeladera = idHeladera, idProducto = idProducto },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static void ActualizarAbiertoProducto(int idProductoXHeladera, bool abierto)
        {
            string sql = "UPDATE ProductoXHeladera SET Abierto = @Abierto WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { Id = idProductoXHeladera, Abierto = abierto });
            }
        }
        public static void IncrementarPopularidadReceta(int idReceta)
        {
            string storedProcedure = "sp_IncrementarPopularidadReceta";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    new { IdReceta = idReceta },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static List<Heladera> BuscarHeladeraDeUsuario(string nombreUsuario)
        {
            List<Heladera> lista = new List<Heladera>();
            string storedProcedure = "BuscarHeladeras";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                lista = connection.Query<Heladera>(
                    storedProcedure,
                    new { Nombre = nombreUsuario },
                    commandType: CommandType.StoredProcedure
                ).ToList();
                }
            return lista;
        }

        public static void CambiarEstado(string nombreEsp, string nuevoEstado)
        {
            string storedProcedure = "CambiarEstadoProducto";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    new { NombreEspecifico = nombreEsp, NuevoEstado = nuevoEstado },
                    commandType: CommandType.StoredProcedure
                );
            }
            return nuevoEstado;
        }
    }
