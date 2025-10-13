public class Heladera {
    public int ID {get;set;}
    public string Color {get;set;}
    public string Nombre {get;set;}
    public Heladera(int ID, string Color, string Nombre)
    {
        this.ID = ID;
        this.Color = Color;
        this.Nombre = Nombre;
    }
    public void CambiarColor(string color){
        Color = color;
    }
    public void CambiarNombre(string nombre){
        Nombre = nombre;
    }
}

