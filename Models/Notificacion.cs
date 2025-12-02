public class Notificacion{
    public int ID{get;set;}
    public string Mensaje{get; set;}
    public DateTime FechaNotificacion{get;set;}
    public bool Leida{get;set;}
    
    public Notificacion(int ID, string Mensaje, DateTime FechaNotificacion){
        this.ID = ID;
        this.Mensaje = Mensaje;
        this.FechaNotificacion = FechaNotificacion;
        this.Leida = false;
        this.Borrada = false; //Nueva propiedad
    }
    public void NotiLeida(){
        int resultado = BD.MarcarNotiComoLeida(ID);

        if (resultado == 1) {
            Leida = true;
        }

        return resultado;
    }

    public int CrearNotificacion(string Mensaje, DateTime FechaNotificacion) {
        
        int ID = BD.CrearNoti(Mensaje, FechaNotificacion); //Se pasa ID como resultado para ponerlo
        
        if (ID != -1) {
            this.ID = ID;
            this.Mensaje = Mensaje;
            this.FechaNotificacion = FechaNotificacion;
            Leida = false;
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
