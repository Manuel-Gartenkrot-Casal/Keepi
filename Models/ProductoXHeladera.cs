public class ProductoXHeladera {
    public int ID {get;set;}
    public int IdHeladera {get;set;}
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
}