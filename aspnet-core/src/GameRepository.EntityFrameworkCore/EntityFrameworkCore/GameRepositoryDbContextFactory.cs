using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GameRepository.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class GameRepositoryDbContextFactory : IDesignTimeDbContextFactory<GameRepositoryDbContext>
{
    public GameRepositoryDbContext CreateDbContext(string[] args)
    {
        GameRepositoryEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<GameRepositoryDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new GameRepositoryDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../GameRepository.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
