using Lab5.Infrastructure.DataAccess.Migrations;
using Lab5.Infrastructure.DataAccess.Services;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace Lab5.Infrastructure.DataAccess.Extentions;

public static class InfrastructureExtention
{
    public static void ExtentionInf(this IServiceCollection nCollection, string connectionString)
    {
        nCollection.AddSingleton<IInitialMigration, InitialMigration>();
        nCollection.AddSingleton<IBdMaster, BdMaster>(x =>
            new BdMaster(connectionString, x.GetRequiredService<IInitialMigration>()));
        nCollection.AddSingleton<IAdminService, AdminService>(x =>
            new AdminService(x.GetRequiredService<IBdMaster>()));
        nCollection.AddSingleton<IUserService, UserService>(x =>
            new UserService(x.GetRequiredService<IBdMaster>()));
        nCollection.AddSingleton<IPersonRepository, PersonRepository>(x =>
            new PersonRepository(x.GetRequiredService<IBdMaster>(), x.GetRequiredService<IUserService>(), x.GetRequiredService<IAdminService>()));
    }
}