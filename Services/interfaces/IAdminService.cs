namespace Services;

public interface IAdminService
{
    public Task<string> GetAdminPassword()
    {
        throw new NotImplementedException();
    }

    public Task<bool> TryChangeAdminPassword(string currentPassword, string newPassword);

    public Task<bool> TryAddUser(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TryAddAdmin(string username)
    {
        throw new NotImplementedException();
    }
}