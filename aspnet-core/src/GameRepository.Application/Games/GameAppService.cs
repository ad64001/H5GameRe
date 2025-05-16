using GameRepository.Games.Dtos;
using GameRepository.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GameRepository.Games
{
    [Authorize(GameRepositoryPermissions.Games.Default)]
    public class GameAppService :
        CrudAppService<Game,GameDto,Guid, GetGameListDto, CreateGameDto, CreateGameDto>,
        IGameAppService
    {
        private readonly GameManager _gameManager;

        public GameAppService(
            IRepository<Game, Guid> repository,
            GameManager gameManager)
            : base(repository)
        {
            _gameManager = gameManager;

            GetPolicyName = GameRepositoryPermissions.Games.Default;
            GetListPolicyName = GameRepositoryPermissions.Games.Default;
            CreatePolicyName = GameRepositoryPermissions.Games.Create;
            UpdatePolicyName = GameRepositoryPermissions.Games.Edit;
            DeletePolicyName = GameRepositoryPermissions.Games.Delete;
        }

        public override async Task<GameDto> CreateAsync(CreateGameDto input)
        {
            var game = await _gameManager.CreateAsync(
                input.Name,
            input.Description,
                input.IconUrl,
                input.GamePath,
                input.EntryFile,
                input.DeveloperName,
                input.Version
            );

            await Repository.InsertAsync(game);

            return ObjectMapper.Map<Game, GameDto>(game);
        }

        [Authorize(GameRepositoryPermissions.Games.Manage)]
        public async Task<GameDto> ChangeStatusAsync(Guid id, UpdateGameStatusDto input)
        {
            await _gameManager.ChangeStatusAsync(id, input.Status);
            var game = await Repository.GetAsync(id);
            return ObjectMapper.Map<Game, GameDto>(game);
        }

        public async Task<string> GetGameEntryUrlAsync(Guid id)
        {
            var game = await Repository.GetAsync(id);
            if (game.Status != GameStatus.Approved)
            {
                throw new GameNotApprovedException(id);
            }

            return $"/games/{game.Id}/{game.EntryFile}";
        }

        protected override async Task<IQueryable<Game>> CreateFilteredQueryAsync(GetGameListDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(g =>
                    g.Name.Contains(input.Filter) ||
                    g.Description.Contains(input.Filter) ||
                    g.DeveloperName.Contains(input.Filter));
            }

            if (input.Status.HasValue)
            {
                query = query.Where(g => g.Status == input.Status.Value);
            }

            return query;
        }
    }

    public class GameNotApprovedException : Exception
    {
        public GameNotApprovedException(Guid id)
            : base($"Game with ID {id} is not approved yet.")
        {
        }
    }
}
