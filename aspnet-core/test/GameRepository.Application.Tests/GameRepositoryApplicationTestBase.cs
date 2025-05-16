using Volo.Abp.Modularity;

namespace GameRepository;

public abstract class GameRepositoryApplicationTestBase<TStartupModule> : GameRepositoryTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
