using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

public class BD 
{
    private static string _connectionString = @"Server=localhost; DataBase=Nombre Base; Integrated Security=True; TrustServerCertificate=True;";

    public double CalcularDuracionProducto(string producto, double D0, double acidez, double agua, double azucar, double conservantes, double alcohol, double porcentajeCambio, int diasAbiertos, double fPromedioBase) {

        double duracionRestante = null; //en controller se tendrían que calcular las fechas exactas
        using (SqlConnection connection = new SqlConnection(_connectionString)) 
        {
            string StoredProcedure = "TeoremaHevia";

            double duracionRestante = connection.QueryFirstOrDefault<double>(
                StoredProcedure,
                new
                {
                    Producto = producto,
                    D0 = D0,
                    Acidez = acidez,
                    Agua = agua,
                    Azucar = azucar,
                    Conservantes = conservantes,
                    Alcohol = alcohol,
                    PorcentajeCambio = porcentajeCambio,
                    DiasAbierto = diasAbierto,
                    f_promedio_base = fPromedioBase
                },
                commandType: CommandType.StoredProcedure
            );
        }
        return duracionRestante;
    }
    public double calcularFPromedioBase() {
        
        var fPromedioBase = null;
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
} 