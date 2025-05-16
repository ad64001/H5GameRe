using GameRepository.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace GameRepository.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(GameRepositoryEntityFrameworkCoreModule),
    typeof(GameRepositoryApplicationContractsModule)
    )]
public class GameRepositoryDbMigratorModule : AbpModule
{
}
