namespace Lab5.Infrastructure.DataAccess.Migrations;

public interface IInitialMigration
{
    public string GetUpSql { get; }
    public string DownUpSql { get; }
}