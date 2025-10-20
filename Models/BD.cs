using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

public class BD
{
    private static string _connectionString = @"Server=localhost; DataBase=Nombre Base; Integrated Security=True; TrustServerCertificate=True;";

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
                    Conservantes = conservantes, //  
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
    public static Usuario verificarUsuario(string userIngresado, string passwordIngresada)
    {
        Usuario user = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string StoredProcedure = "verificarUsuario";
            user = connection.Query<Usuario>(StoredProcedure, new { Username = userIngresado, Password = passwordIngresada }, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }
        return user;

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
    public static int borrarHeladera(string nombre) {
        int resultado = -1;
    
        using (SqlConnection connection = new SqlConnection(_connectionString)) {
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
}