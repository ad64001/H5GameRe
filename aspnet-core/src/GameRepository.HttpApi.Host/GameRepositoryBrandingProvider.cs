using GameRepository.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace GameRepository;

[Dependency(ReplaceServices = true)]
public class GameRepositoryBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<GameRepositoryResource> _localizer;

    public GameRepositoryBrandingProvider(IStringLocalizer<GameRepositoryResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
