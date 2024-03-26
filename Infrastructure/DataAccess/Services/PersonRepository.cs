using Models;
using Npgsql;
using Services;

namespace Lab5.Infrastructure.DataAccess.Services;

public class PersonRepository : IPersonRepository
{
    private IBdMaster _bdMaster = new BdMaster();
    private IUserService _userService;
    private IAdminService _adminService;

    public PersonRepository(IBdMaster bdMaster, IUserService userService, IAdminService adminService)
    {
        _bdMaster = bdMaster;
        _userService = userService;
        _adminService = adminService;
    }

    public async Task<IPerson?> FindByUsername(string username)
    {
        await _bdMaster.ConnectWithDB();

        using var cmd = new NpgsqlCommand("SELECT * FROM persons WHERE person_name = ($1)", _bdMaster.Connection)
        {
            Parameters =
            {
                new() { Value = username },
            },
        };
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            if (reader[3].ToString() == "user")
            {
                IPerson user = new User(reader.GetString(1), reader.GetString(2), reader.GetInt32(0));
                await _bdMaster.DisconnectWithDB();
                return new User(user.Username, user.Password, user.Id, await _userService.GetBalance(user.Id));
            }

            if (reader.GetString(3) == "admin")
            {
                IPerson admin = new Admin(reader.GetString(1), reader.GetString(2), reader.GetInt32(0));
                await _bdMaster.DisconnectWithDB();
                return new Admin(admin.Username, await _adminService.GetAdminPassword(), admin.Id);
            }
        }

        await _bdMaster.DisconnectWithDB();
        return null;
    }

    public async Task<IPerson?> FindById(int id)
    {
        await _bdMaster.ConnectWithDB();

        using var cmd = new NpgsqlCommand("SELECT * FROM persons WHERE id = ($1)", _bdMaster.Connection)
        {
            Parameters =
            {
                new() { Value = id },
            },
        };
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            if (reader[3].ToString() == "user")
            {
                IPerson user = new User(reader.GetString(1), reader.GetString(2), reader.GetInt32(0));
                await _bdMaster.DisconnectWithDB();
                return new User(user.Username, user.Password, user.Id, await _userService.GetBalance(user.Id));
            }

            if (reader.GetString(3) == "admin")
            {
                IPerson admin = new Admin(reader.GetString(1), reader.GetString(2), reader.GetInt32(0));
                await _bdMaster.DisconnectWithDB();
                return new Admin(admin.Username, await _adminService.GetAdminPassword(), admin.Id);
            }
        }

        await _bdMaster.DisconnectWithDB();
        return null;
    }
}