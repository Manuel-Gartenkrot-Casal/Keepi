public class Notificacion{
    public int ID{get;set;}
    public string Mensaje{get; set;}
    public DateTime FechaNotificacion{get;set;}
    public bool Leida{get;set;}
    public int IdProductoXHeladera {get;set;} //Nueva propiedad
    public bool Borrada{get;set;} //Nueva propiedad
    
    public Notificacion(){
    }

    public Notificacion(int ID, string Mensaje, DateTime FechaNotificacion, int IdProductoXHeladera){
        this.ID = ID;
        this.Mensaje = Mensaje;
        this.FechaNotificacion = FechaNotificacion;
        this.Leida = false;
        this.IdProductoXHeladera = IdProductoXHeladera;
        this.Borrada = false; //Nueva propiedad
    }
    
    public int NotiLeida() {
        int resultado = BD.MarcarNotiComoLeida(ID);

        if (resultado == 1) {
            Leida = true;
        }

        return resultado;
    }

    public int CrearNotificacion(string Mensaje, DateTime FechaNotificacion, int IdProductoXHeladera) {
        
        int ID = BD.CrearNoti(Mensaje, FechaNotificacion, IdProductoXHeladera); //Se pasa ID como resultado para ponerlo
        
        if (ID != -1) {
            this.ID = ID;
            this.Mensaje = Mensaje;
            this.FechaNotificacion = FechaNotificacion;
            Leida = false;
            this.IdProductoXHeladera = IdProductoXHeladera;
            Borrada = false;
        }

        return ID;
    }

    public int BorrarNotificacion() {
        int resultado = BD.BorrarNoti(ID);

        if (resultado == 1) {
            Borrada = true; //Incluir en la lógica
        }

        return resultado;
    }
}

