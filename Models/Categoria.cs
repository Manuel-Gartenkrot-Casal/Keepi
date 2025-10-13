class Categoria {
    public int ID {get;set;}
    public string Nombre {get;set;}
    public double PorcCambio {get;set;}

    public Categoria(int ID, string Nombre, double PorcCambio) {
        this.ID = ID;
        this.Nombre = Nombre;
        this.PorcCambio = PorcCambio;
    }
}
