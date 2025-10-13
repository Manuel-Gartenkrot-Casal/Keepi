class Usuario{
    public int ID {get;set;}
    public string Username {get;set;}
    public string Password {get;set;}
    public Usuario (int ID, string Username, string Password)
    {
        this.ID = ID;
        this.Username = Username;
        this.Password = Password;
    }
}