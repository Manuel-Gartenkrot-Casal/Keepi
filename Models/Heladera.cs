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
        this.Color = Color ?? "#7ecb20";
        this.Nombre = Nombre ?? $"Heladera{ID}";
        this.Eliminado = false;
    }
    public void CambiarColor(string color){
        Color = color;
    }
    public void CambiarNombre(string nombre){
        Nombre = nombre;
    }
    
    public int EliminarHeladera(int idUsuario)
    {
        int resultado = BD.borrarHeladera(this.ID, idUsuario);
        
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
