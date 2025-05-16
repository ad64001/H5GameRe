using Volo.Abp.Application.Dtos;

namespace GameRepository.Games.Dtos
{
    public class GetGameListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public GameStatus? Status { get; set; }
    }
}
