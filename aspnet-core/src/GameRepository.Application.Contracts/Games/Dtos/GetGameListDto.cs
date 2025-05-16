using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GameRepository.Games.Dtos
{
    public class GetGameListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public GameStatus? Status { get; set; }
    }
}
