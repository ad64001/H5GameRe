using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GameRepository.Data;
using Volo.Abp.DependencyInjection;

namespace GameRepository.EntityFrameworkCore;

public class EntityFrameworkCoreGameRepositoryDbSchemaMigrator
    : IGameRepositoryDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreGameRepositoryDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the GameRepositoryDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<GameRepositoryDbContext>()
            .Database
            .MigrateAsync();
    }
}
