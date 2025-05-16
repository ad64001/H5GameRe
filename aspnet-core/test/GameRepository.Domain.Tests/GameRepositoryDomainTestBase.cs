using Volo.Abp.Modularity;

namespace GameRepository;

/* Inherit from this class for your domain layer tests. */
public abstract class GameRepositoryDomainTestBase<TStartupModule> : GameRepositoryTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
