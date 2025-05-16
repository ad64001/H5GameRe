using GameRepository.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace GameRepository.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class GameRepositoryController : AbpControllerBase
{
    protected GameRepositoryController()
    {
        LocalizationResource = typeof(GameRepositoryResource);
    }
}
