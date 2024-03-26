using System.Data;
using Lab5.Infrastructure.DataAccess.Migrations;
using Npgsql;

namespace Lab5.Infrastructure.DataAccess.Services;

#pragma warning disable CA2100
public class BdMaster : IBdMaster
{
    private string _connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

    private IInitialMigration _initialMigrationMigration = new InitialMigration();

    public BdMaster(string connectionString, IInitialMigration initialMigrationMigration)
    {
        _connectionString = connectionString;
        _initialMigrationMigration = initialMigrationMigration;
    }

    public BdMaster()
    {
    }

    public NpgsqlConnection? Connection { get; private set; }

    public async Task ConnectWithDB()
    {
        Connection = new NpgsqlConnection(_connectionString);
        await Connection.OpenAsync();
        await using var command = new NpgsqlCommand(_initialMigrationMigration.GetUpSql, Connection);
        await command.ExecuteNonQueryAsync();
        await Connection.CloseAsync();
        await Connection.OpenAsync();
/*
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        NpgsqlDataSource dataSource = dataSourceBuilder.Build();

        Connection = await dataSource.OpenConnectionAsync();

        await using NpgsqlCommand command = dataSource.CreateCommand(_initialMigrationMigration.GetUpSql);
        await command.ExecuteNonQueryAsync();
        */
    }

    public async Task DisconnectWithDB()
    {
        if (Connection is null) return;
        if (Connection.State == ConnectionState.Open)
        {
            await Connection.CloseAsync();
        }
    }
}