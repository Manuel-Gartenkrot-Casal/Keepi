 public class ProductoXReceta {
    public int ID {get;set;}
    public int IdProducto {get;set;}
    public int IdReceta {get;set;}
    public double CantNecesaria {get;set;}

        public ProductoXReceta(int ID, int IdProducto, int IdReceta, double CantNecesaria) {
        this.ID = ID;
        this.IdProducto = IdProducto;
        this.IdReceta = IdReceta;
        this.CantNecesaria = CantNecesaria;
    }
}
