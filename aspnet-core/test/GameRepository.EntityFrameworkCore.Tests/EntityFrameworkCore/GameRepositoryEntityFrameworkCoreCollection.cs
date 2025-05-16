using Xunit;

namespace GameRepository.EntityFrameworkCore;

[CollectionDefinition(GameRepositoryTestConsts.CollectionDefinitionName)]
public class GameRepositoryEntityFrameworkCoreCollection : ICollectionFixture<GameRepositoryEntityFrameworkCoreFixture>
{

}
