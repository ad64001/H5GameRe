using GameRepository.Samples;
using Xunit;

namespace GameRepository.EntityFrameworkCore.Domains;

[Collection(GameRepositoryTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<GameRepositoryEntityFrameworkCoreTestModule>
{

}
