public class UsuarioXHeladera {
    public int ID {get;set;}
    public int IdUsuario {get;set;}
    public int IdHeladera {get;set;}
    public bool Duenio {get;set;}

    public UsuarioXHeladera(int ID, int IdUsuario, int IdHeladera, bool Duenio) {
        this.ID = ID;
        this.IdUsuario = IdUsuario;
        this.IdHeladera = IdHeladera;
        this.Duenio = Duenio;
    }
}

