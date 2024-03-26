using Npgsql;
using NpgsqlTypes;
using Services;

namespace Lab5.Infrastructure.DataAccess.Services;

public class AdminService : IAdminService
{
    private IBdMaster _bdMaster = new BdMaster();

    public AdminService(IBdMaster bdMaster)
    {
        _bdMaster = bdMaster;
    }

    public async Task<string> GetAdminPassword()
    {
        await _bdMaster.ConnectWithDB();

        using var cmd = new NpgsqlCommand("SELECT password_value FROM admin_password", _bdMaster.Connection);
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            string ans = reader.GetString(0);
            await _bdMaster.DisconnectWithDB();
            return ans;
        }

        await _bdMaster.DisconnectWithDB();
        return string.Empty;
    }

    public async Task<bool> TryChangeAdminPassword(string currentPassword, string newPassword)
    {
        if (string.IsNullOrEmpty(currentPassword) || !currentPassword.Equals(await GetAdminPassword(), StringComparison.Ordinal))
        {
            return false;
        }

        await _bdMaster.ConnectWithDB();

        using var command = new NpgsqlCommand("UPDATE admin_password SET password_value = ($1)", _bdMaster.Connection)
        {
            Parameters =
            {
                new() { Value = newPassword },
            },
        };
        await command.ExecuteNonQueryAsync();
        return true;
    }

    public async Task<bool> TryAddUser(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        try
        {
            await AddUser(username, password);
            return true;
        }
        catch (Exception e) when (e is Exception)
        {
            return false;
        }
    }

    public async Task<bool> TryAddAdmin(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return false;
        }

        string password = await GetAdminPassword();

        try
        {
            await _bdMaster.ConnectWithDB();

            using var command = new NpgsqlCommand("INSERT INTO persons (person_name, person_password, role) VALUES (($1),($2), \'admin\')", _bdMaster.Connection)
            {
                Parameters =
                {
                    new() { Value = username },
                    new() { Value = password },
                },
            };
            await command.ExecuteNonQueryAsync();
            await _bdMaster.DisconnectWithDB();
            return true;
        }
        catch (Exception e) when (e is Exception)
        {
            return false;
        }
    }

    private async Task AddUser(string username, string password)
    {
        await _bdMaster.ConnectWithDB();

        await using var command =
            new NpgsqlCommand(
                "INSERT INTO persons (person_name, person_password, role) VALUES ((@username),(@password), \'user\')",
                _bdMaster.Connection);

        command.Parameters.AddWithValue("@username", NpgsqlDbType.Text, username);
        command.Parameters.AddWithValue("@password", NpgsqlDbType.Text, password);
        await command.ExecuteNonQueryAsync();

        command.CommandText = "SELECT id FROM persons WHERE person_name = (@username)";

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        int ind = 0;
        while (await reader.ReadAsync())
        {
            ind = reader.GetInt32(0);
        }

        await reader.CloseAsync();
        command.Parameters.AddWithValue("@index", NpgsqlDbType.Bigint, ind);
        command.CommandText =
            "INSERT INTO users_info (id, name, password, balance) VALUES ((@index),(@username),(@password), 0)";
        await command.ExecuteNonQueryAsync();
        await _bdMaster.DisconnectWithDB();
    }
}