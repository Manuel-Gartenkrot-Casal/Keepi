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
                            Where u.Id = @UsuarioId
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
    }
}