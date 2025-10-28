namespace Keepi.Models;
public class ProductoXHeladera {
    public int ID {get;set;}
    public int IdHeladera {get;set;}
    public int IdProducto {get;set;}
        public int IdUsuario {get;set;}

    public string NombreEsp {get;set;}
    public DateTime FechaVencimiento {get;set;}
    public bool Eliminado {get;set;}
    public bool Abierto {get;set;}
    public string Foto {get;set;}
    
    public ProductoXHeladera(int ID, int IdHeladera, int IdUsuario, string NombreEsp, DateTime FechaVencimiento, bool Eliminado, bool Abierto, string Foto) {
        this.ID = ID;
        this.IdHeladera = IdHeladera;
        this.IdUsuario = IdUsuario;
        this.NombreEsp = NombreEsp;
        this.FechaVencimiento = FechaVencimiento;  
        this.Eliminado = Eliminado;
        this.Abierto = Abierto;
        this.Foto = Foto;
    }
    public int ObtenerDiasRestantes()
    {
        int diasRestantes = (FechaVencimiento.Date - DateTime.Now.Date).Days;

        if (diasRestantes < 0)
            {
                Console.WriteLine("Hay menos de cero días restantes");
                return -1;
            }
            else if (diasRestantes == 0)
            {
                Console.WriteLine("Hay cero días restantes");
                return diasRestantes;
            }
            else
            {
                Console.WriteLine("Se lograron pasar los días restantes");
                return diasRestantes;
            }

    }
    /*public double CalcularPorcentajeRestante() //Para barra de progreso, otros calculos como notificaciones
    {
        Producto productoBase = Objeto.StringToObject<Producto>(HttpContext.Session.GetString("IdProducto"));
        int DiasBase = productoBase.Duracion;
        int DiasRestantes = ObtenerDiasRestantes();
        double porcentaje = ((double)DiasRestantes / DiasBase) * 100;
        return porcentaje;
    }*/
    public void ProductoVencido() { //hacer para que se pueda vencer el producto y cambiar el mensaje de quedan x dias por Producto Vencido en caso de que el usuario no elija descartar el producto

    }
}