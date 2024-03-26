using Npgsql;

namespace Lab5.Infrastructure.DataAccess.Services;

public interface IBdMaster
{
    public NpgsqlConnection? Connection { get; }

    public Task ConnectWithDB();

    public Task DisconnectWithDB();
}