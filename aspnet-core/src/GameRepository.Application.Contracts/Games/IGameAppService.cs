using GameRepository.Games.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GameRepository.Games
{
    public interface IGameAppService :
         ICrudAppService<
             GameDto,
             Guid,
             GetGameListDto,
             CreateGameDto,
             CreateGameDto>
    {
        Task<GameDto> ChangeStatusAsync(Guid id, UpdateGameStatusDto input);
        Task<string> GetGameEntryUrlAsync(Guid id);
    }
}
