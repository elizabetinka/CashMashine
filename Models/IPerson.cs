namespace Models;

public interface IPerson
{
    public string Role { get; }

    public string Username { get; }

    public string Password { get; }
    public int Id { get; }
}