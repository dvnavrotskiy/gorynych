using FluentMigrator.Runner;

namespace gorynych.api.Dal.Migrations;

public static class MigrationRunner
{
    public static void Up(IConfiguration cfg)
    {
        var connectionString = cfg.GetConnectionString("messagesConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Messages Connection string not found");
        
        var runner = CreateServiceProvider(connectionString)
            .GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
    
    private static ServiceProvider CreateServiceProvider(string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(
                rb => rb.AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(MigrationRunner).Assembly)
                    .For.All()
            )
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }
}