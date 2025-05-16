using Volo.Abp.Modularity;

namespace GameRepository;

[DependsOn(
    typeof(GameRepositoryApplicationModule),
    typeof(GameRepositoryDomainTestModule)
)]
public class GameRepositoryApplicationTestModule : AbpModule
{

}
