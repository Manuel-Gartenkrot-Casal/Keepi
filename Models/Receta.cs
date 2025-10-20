public class Receta{
public int ID{get; set;}
public string nombre{get; set;}
public bool favorito{get; set;}
public int duracion{get; set;}

    public Receta(int ID, string nombre, bool favorito, int duracion){
    this.ID = ID;
    this.nombre = nombre;
    this.favorito = favorito;
    this.duracion = duracion;
    }
    public void PonerFavorito(){
        favorito = true;
    }
}