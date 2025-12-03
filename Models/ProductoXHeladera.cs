namespace Keepi.Models;
public class ProductoXHeladera {
    public int ID {get;set;}
    public int IdHeladera {get;set;}
    public int IdProducto {get;set;}
    public int IdUsuario {get;set;}
    public string NombreEspecifico {get;set;}
    public string? NombreProducto {get;set;} 
    public DateTime FechaVencimiento {get;set;}
    public bool Eliminado {get;set;}
    public bool Abierto {get;set;}
    public string Foto {get;set;}
    public string Estado {get;set;}
    
    public ProductoXHeladera() {
    }
    
    public ProductoXHeladera(int ID, int IdHeladera, int IdProducto, int idUsuario, string NombreEspecifico, string NombreProducto, DateTime FechaVencimiento, bool Eliminado, bool Abierto, string Foto, string Estado) {
        this.ID = ID;
        this.IdHeladera = IdHeladera;
        this.IdProducto = IdProducto;
        this.IdUsuario = IdUsuario;
        this.NombreEspecifico = NombreEspecifico;
        this.NombreProducto = NombreProducto;
        this.FechaVencimiento = FechaVencimiento;  
        this.Eliminado = Eliminado;
        this.Abierto = Abierto;
        this.Foto = Foto;
        this.Estado = Estado;
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

    public double CalcularPorcentajeVencimiento(int duracionTotal)
    {
        if (duracionTotal <= 0) return 0;
        int diasRestantes = ObtenerDiasRestantes();
        if (diasRestantes < 0) return 0;
        double porcentaje = ((double)diasRestantes / duracionTotal) * 100;
        return Math.Max(0, Math.Min(100, porcentaje));
    }

    public int ObtenerDiasDesdeCarga(int duracionTotal)
    {
        int diasRestantes = ObtenerDiasRestantes();
        if (diasRestantes < 0) return duracionTotal;
        return duracionTotal - diasRestantes;
    }
    public double CalcularPorcentajeRestante() //Para barra de progreso, otros calculos como notificaciones
    {
        Producto productoBase = BD.TraerProductoByID(IdProducto);
        int DiasBase = productoBase.Duracion;
        int DiasRestantes = ObtenerDiasRestantes();
        double porcentaje = ((double)DiasRestantes / DiasBase) * 100;
        return porcentaje;
    }
    public void ProductoVencido() { //hacer para que se pueda vencer el producto y cambiar el mensaje de quedan x dias por Producto Vencido en caso de que el usuario no elija descartar el producto

    }
}
