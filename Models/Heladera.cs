public class Heladera {
    public int ID {get;set;}
    public string Color {get;set;}
    public string Nombre {get;set;}
    public bool Eliminado {get;set;}

    public Heladera(int ID, string Color, string Nombre)
    {
        this.ID = ID;
        this.Color = Color ?? "#D3D3D3";
        this.Nombre = Nombre ?? $"Heladera{ID}";
        this.Eliminado = false;
    }
    public void CambiarColor(string color){
        Color = color;
    }
    public void CambiarNombre(string nombre){
        Nombre = nombre;
    }
    public int EliminarHeladera(string nombreUsuario)
    {
        int resultado = BD.borrarHeladera(Nombre, nombreUsuario);
        
        if (resultado == -1) 
        {
            Eliminado = false;
        }
        else {
            Eliminado = true;
        }
        
        return resultado;
    }
}

