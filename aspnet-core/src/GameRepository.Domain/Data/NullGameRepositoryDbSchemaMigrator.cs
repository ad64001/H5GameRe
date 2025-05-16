using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace GameRepository.Data;

/* This is used if database provider does't define
 * IGameRepositoryDbSchemaMigrator implementation.
 */
public class NullGameRepositoryDbSchemaMigrator : IGameRepositoryDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
