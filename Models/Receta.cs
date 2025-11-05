public class Receta{
public int ID{get; set;}
public string nombre{get; set;}
public bool favorito{get; set;}
public int duracion{get; set;}
public int Popularidad{get; set;}
public int Raciones{get; set;}
<<<<<<< HEAD
public string Dificultad{get;set;}

public Receta (){}


=======
public string Dificultad{get;set;}  
>>>>>>> 228866b44692838ac092f554f15056a0e6b69441
    public Receta(int ID, string nombre, bool favorito, int duracion, int Popularidad, int Raciones, string Dificultad){
    this.ID = ID;
    this.nombre = nombre;
    this.favorito = favorito;
    this.duracion = duracion;
    this.Popularidad = Popularidad;
    this.Raciones = Raciones;
    this.Dificultad = Dificultad;
    }
    public void PonerFavorito(){
        favorito = true;
    }
}