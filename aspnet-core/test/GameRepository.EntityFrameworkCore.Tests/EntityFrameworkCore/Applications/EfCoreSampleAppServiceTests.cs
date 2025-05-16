using GameRepository.Samples;
using Xunit;

namespace GameRepository.EntityFrameworkCore.Applications;

[Collection(GameRepositoryTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<GameRepositoryEntityFrameworkCoreTestModule>
{

}
