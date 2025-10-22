 public class Usuario
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }

    public Usuario(int ID, string Username, string Password, string Nombre, string Apellido, string Email)
    {
        this.ID = ID;
        this.Username = Username;
        this.Password = Password;
        this.Nombre = Password;
        this.Apellido = Password;
        this.Email = Password;
    }
}