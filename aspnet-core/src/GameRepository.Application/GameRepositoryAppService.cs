using System;
using System.Collections.Generic;
using System.Text;
using GameRepository.Localization;
using Volo.Abp.Application.Services;

namespace GameRepository;

/* Inherit your application services from this class.
 */
public abstract class GameRepositoryAppService : ApplicationService
{
    protected GameRepositoryAppService()
    {
        LocalizationResource = typeof(GameRepositoryResource);
    }
}
