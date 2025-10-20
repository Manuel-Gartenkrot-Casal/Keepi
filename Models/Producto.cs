public class Producto {
    public int ID {get;set;}
    public int IdCategoria {get;set;}
    public string Nombre {get;set;}
    public int Duracion {get;set;}
    public bool Favorito {get;set;}
    public double PorcAcidez {get;set;}
    public double PorcAgua {get;set;}
    public double PorcAzucar {get;set;}
    public double PorcConservantes {get;set;}
    public double PorcAlcohol {get;set;}

    public Producto(int ID, string Nombre, int Duracion, bool Favorito, double PorcAcidez, double PorcAgua, double PorcAzucar, double PorcConservantes, double PorcAlcohol) {
        this.ID = ID;
        this.Nombre = Nombre;
        this.Duracion = Duracion;
        this.Favorito = Favorito;
        this.PorcAcidez = PorcAcidez;
        this.PorcAgua = PorcAgua;
        this.PorcAzucar = PorcAzucar;
        this.PorcConservantes = PorcConservantes;
        this.PorcAlcohol = PorcAlcohol;
    }
    public void ponerFavorito() {
        Favorito = true;
    }
}