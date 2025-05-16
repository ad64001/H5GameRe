using System.Threading.Tasks;

namespace GameRepository.Data;

public interface IGameRepositoryDbSchemaMigrator
{
    Task MigrateAsync();
}
