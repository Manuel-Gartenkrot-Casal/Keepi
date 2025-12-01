public class Heladera {
    public int ID {get;set;}
    public string Color {get;set;}
    public string Nombre {get;set;}
    public bool Eliminado {get;set;}

    public Heladera(){
        
    }
    public Heladera(int ID, string Color, string Nombre)
    {
        this.ID = ID;
        this.Color = Color ?? "#D3D3D3";
        this.Nombre = Nombre ?? $"Heladera{ID}";
        this.Eliminado = false;
    }
    public void CambiarColor(string color, int id)
    {
        Color = color;
        BD.CambiarColorHeladera(id, color);
    }
    public void CambiarNombre(string nombre, int id)
    {
        Nombre = nombre;
        BD.CambiarNombreHeladera(id, nombre);
    }
    public int EliminarHeladera(string nombreUsuario)
    {
        int resultado = BD.borrarHeladera(Nombre);
        
        if (resultado == -1) 
        {
            Eliminado = false;
        }
        else {
            Eliminado = true;
        }
        
        return resultado;
    }

    public int AgregarHeladera(string nombre, string color)
    {
        int resultado = BD.agregarHeladera(nombre, color);
        
        if (resultado == 1)
        {
            this.Nombre = nombre;
            this.Color = color;
            this.Eliminado = false;
        }
        
        return resultado;
    }
}


