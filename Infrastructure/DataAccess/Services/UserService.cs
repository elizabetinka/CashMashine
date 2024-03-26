using System.Globalization;
using Npgsql;
using Services;

namespace Lab5.Infrastructure.DataAccess.Services;

public class UserService : IUserService
{
    private IBdMaster _bdMaster = new BdMaster();

    public UserService(IBdMaster bdMaster)
    {
        _bdMaster = bdMaster;
    }

    public async Task<IList<string>> GetOperationsHistory(int id, int operationsCount = 10)
    {
        IList<string> ans = new List<string>();

        await _bdMaster.ConnectWithDB();

        using var cmd = new NpgsqlCommand("SELECT date, name, operation FROM operations_history LEFT JOIN users_info on operations_history.user_id = users_info.id WHERE user_id = ($1) LIMIT ($2)", _bdMaster.Connection)
        {
            Parameters =
            {
                new() { Value = id },
                new() { Value = operationsCount },
            },
        };
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            ans.Add("Дата: " + reader.GetDateTime(0).ToString(CultureInfo.CurrentCulture) + " Имя: " + reader.GetString(1) + " Операция: " +
                    reader.GetInt32(2));
        }

        await _bdMaster.DisconnectWithDB();
        return ans;
    }

    public async Task<bool> TryAddCash(int money, int id)
    {
        try
        {
            await _bdMaster.ConnectWithDB();
            if (_bdMaster.Connection is null) throw new NpgsqlException();
            await using NpgsqlTransaction transaction = await _bdMaster.Connection.BeginTransactionAsync();

            await using var command = new NpgsqlCommand("UPDATE users_info SET balance = balance + ($1) WHERE id = ($2)", _bdMaster.Connection)
            {
                Parameters =
                {
                    new() { Value = money },
                    new() { Value = id },
                },
            };
            await command.ExecuteNonQueryAsync();

            await using var cmd = new NpgsqlCommand("INSERT INTO operations_history (user_id, operation, date) VALUES (($1),($2), CURRENT_DATE)", _bdMaster.Connection)
            {
                Parameters =
                {
                    new() { Value = id },
                    new() { Value = money },
                },
            };
            await cmd.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
            await _bdMaster.DisconnectWithDB();
            return true;
        }
        catch (Exception e) when (e is Exception)
        {
            await _bdMaster.DisconnectWithDB();
            return false;
        }
    }

    public async Task<bool> TryTakeCash(int money, int id)
    {
        try
        {
            await _bdMaster.ConnectWithDB();
            if (_bdMaster.Connection is null) throw new NpgsqlException();
            await using NpgsqlTransaction transaction = await _bdMaster.Connection.BeginTransactionAsync();

            await using var command = new NpgsqlCommand("UPDATE users_info SET balance = balance - ($1) WHERE id = ($2)", _bdMaster.Connection)
            {
                Parameters =
                {
                    new() { Value = money },
                    new() { Value = id },
                },
            };
            await command.ExecuteNonQueryAsync();

            await using var cmd = new NpgsqlCommand("INSERT INTO operations_history (user_id, operation, date) VALUES (($1),($2), CURRENT_DATE)", _bdMaster.Connection)
            {
                Parameters =
                {
                    new() { Value = id },
                    new() { Value = -money },
                },
            };
            await cmd.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
            await _bdMaster.DisconnectWithDB();
            return true;
        }
        catch (Exception e) when (e is Exception)
        {
            await _bdMaster.DisconnectWithDB();
            return false;
        }
    }

    public async Task<int> GetBalance(int idx)
    {
        await _bdMaster.ConnectWithDB();

        using var cmd = new NpgsqlCommand("SELECT balance FROM users_info WHERE id = ($1)", _bdMaster.Connection)
        {
            Parameters =
            {
                new() { Value = idx },
            },
        };
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
        int ans = 0;
        while (await reader.ReadAsync())
        {
            ans = reader.GetInt32(0);
            await _bdMaster.DisconnectWithDB();
            return ans;
        }

        await _bdMaster.DisconnectWithDB();
        return ans;
    }
}