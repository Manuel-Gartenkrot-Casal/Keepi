using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Keepi.Models;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Keepi.Services
{
    public interface INotificacionService
    {
        List<Notificacion> ObtenerNotificacionesDelUsuario(int usuarioId);

        Task GenerarNotificacionesAutomaticas();
    }

    public class NotificacionService : INotificacionService
    {

        private readonly string _connectionString;

        public NotificacionService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Notificacion> ObtenerNotificacionesDelUsuario(int usuarioId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open(); // valida que la conexión funcione

                string sql = @"Select n.* 
                            From Notificacion n
                            Inner Join ProductoXHeladera ph On n.IdProductoxHeladera = ph.Id
                            Inner Join Heladera h ON ph.IdHeladera = h.Id
                            Inner Join UsuarioXHeladera uh On h.Id = uh.IdHeladera
                            Inner Join Usuario u ON uh.IdUsuario = u.Id
                            Where u.Id = @UsuarioId AND n.Borrada = 0
                            Order By n.FechaNotificacion Desc";

                return connection.Query<Notificacion>(sql, new { UsuarioId = usuarioId }).ToList();
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error de SQL al obtener notificaciones: " + sqlEx.Message);
                return new List<Notificacion>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inesperado al obtener notificaciones: " + ex.Message);
                return new List<Notificacion>();
            }

        }

        public async Task GenerarNotificacionesAutomaticas()
        {
            using var connection = new SqlConnection(_connectionString);

            string sql = @"
                SELECT p.*, n.Id AS NotiExistente
                FROM ProductoXHeladera p
                LEFT JOIN Notificacion n 
                    ON n.IdProductoXHeladera = p.Id
                    AND n.Borrada = 0
            ";

            var productos = (await connection.QueryAsync<dynamic>(sql)).ToList();

            foreach (var p in productos)
            {
                int dias = ObtenerDiasRestantes((DateTime)p.FechaVencimiento);

                double cantidadActual = Convert.ToDouble(p.CantidadActual);
                double cantidadInicial = Convert.ToDouble(p.CantidadInicial);

                double porcentaje = CalcularPorcentajeRestante(cantidadActual, cantidadInicial);
                
                int porcentajeInt = (int)Math.Round(porcentaje);

                bool yaTieneNoti = p.NotiExistente != null;

                if (!yaTieneNoti)
                {
                    string mensaje = ObtenerMensaje(p.NombreEspecifico, dias, porcentajeInt);

                    if (mensaje != null)
                    {
                        string insert = @"
                            INSERT INTO Notificacion 
                                (Mensaje, FechaNotificacion, Leida, IdProductoXHeladera, Borrada)
                            VALUES (@Mensaje, @Fecha, 0, @Id, 0)
                        ";

                        await connection.ExecuteAsync(insert, new {
                            Mensaje = mensaje,
                            Fecha = DateTime.Now.Date,
                            Id = (int)p.Id
                        });
                    }
                }
            }
        }

        private int ObtenerDiasRestantes(DateTime fechaVencimiento)
        {
            return (fechaVencimiento.Date - DateTime.Now.Date).Days;
        }

        private double CalcularPorcentajeRestante(double cantidadActual, double cantidadInicial)
        {
            return (cantidadInicial == 0) ? 0 : (cantidadActual / cantidadInicial) * 100;
        }

        private string ObtenerMensaje(string nombre, int dias, int porcentajeInt)
        {
            if ((porcentajeInt == 50 || porcentajeInt == 25 || porcentajeInt == 10) && dias != 1)
                return $"Faltan {dias} días para el vencimiento de {nombre}.";

            if (dias == 1)
                return $"Falta 1 día para el vencimiento de {nombre}.";

            return null;
        }

    }
}
