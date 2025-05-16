using Volo.Abp.Modularity;

namespace GameRepository;

[DependsOn(
    typeof(GameRepositoryDomainModule),
    typeof(GameRepositoryTestBaseModule)
)]
public class GameRepositoryDomainTestModule : AbpModule
{

}
