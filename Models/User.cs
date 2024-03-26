namespace Models;

public class User : IPerson
{
    public User(string username, string password, int id, int balance = 0)
    {
        Username = username;
        Password = password;
        Id = id;
        Balance = balance;
    }

    public string Role { get; private set; } = "user";
    public string Username { get; }
    public string Password { get; }
    public int Id { get; }
    public int Balance { get; private set; }

    public bool DecreaseMoney(int money)
    {
        if (money > Balance)
        {
            return false;
        }

        Balance -= money;
        return true;
    }

    public bool IncreaseMoney(int money)
    {
        Balance += money;
        return true;
    }
}