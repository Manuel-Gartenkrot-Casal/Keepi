public class Receta{
public int ID{get; set;}
public string nombre{get; set;}
public bool favorito{get; set;}
public int duracion{get; set;}
public int Popularidad{get; set;}
public int Raciones{get; set;}
public string Dificultad{get;set;}
    public Receta(int ID, string nombre, bool favorito, int duracion, int Popularidad, int Raciones, string Dificultad){
    this.ID = ID;
    this.nombre = nombre;
    this.favorito = favorito;
    this.duracion = duracion;
    this.Raciones = Raciones;
    this.Dificultad = Dificultad;
    }
    public void PonerFavorito(){
        favorito = true;
    }
}