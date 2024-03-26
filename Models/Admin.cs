namespace Models;

public class Admin : IPerson
{
    public Admin(string username, string password, int id)
    {
        Username = username;
        Password = password;
        Id = id;
    }

    public string Role { get; private set; } = "admin";
    public string Username { get; }
    public string Password { get; }
    public int Id { get; }
}