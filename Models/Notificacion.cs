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
    }
    public void NotiLeida(){
        Leida = true;
    }
}