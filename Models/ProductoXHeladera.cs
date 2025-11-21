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
    public string TipoAlmacenamiento {get;set;} = "Refrigerado";
    
    public ProductoXHeladera() {
    }
    
    public ProductoXHeladera(int ID, int IdHeladera, int IdUsuario, string NombreEspecifico, DateTime FechaVencimiento, bool Eliminado, bool Abierto, string Foto) {
        this.ID = ID;
        this.IdHeladera = IdHeladera;
        this.IdUsuario = IdUsuario;
        this.NombreEspecifico = NombreEspecifico;
        this.FechaVencimiento = FechaVencimiento;  
        this.Eliminado = Eliminado;
        this.Abierto = Abierto;
        this.Foto = Foto;
        this.TipoAlmacenamiento = "Refrigerado";
    }
    
    public int ObtenerDiasRestantes()
    {
        int diasRestantes = (FechaVencimiento.Date - DateTime.Now.Date).Days;
        return diasRestantes;
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
}
