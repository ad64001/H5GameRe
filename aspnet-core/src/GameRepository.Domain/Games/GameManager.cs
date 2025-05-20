using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace GameRepository.Games
{
    public class GameManager : DomainService
    {
        private readonly IRepository<Game, Guid> _gameRepository;

        public GameManager(IRepository<Game, Guid> gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Game> CreateAsync(
            string name,
            string description,
            string iconUrl,
            string gamePath,
            string entryFile,
            string developerName,
            string version)
        {
            var game = new Game
            {
                Name = name,
                Description = description,
                IconUrl = iconUrl,
                GamePath = gamePath,
                EntryFile = entryFile,
                DeveloperName = developerName,
                Version = version
            };

            return await _gameRepository.InsertAsync(game);
        }

        public async Task ChangeStatusAsync(Guid id, GameStatus status)
        {
            var game = await _gameRepository.GetAsync(id);
            game.Status = status;
            await _gameRepository.UpdateAsync(game);
        }

        public async Task UpdateAsync(
            Game game,
            string name,
            string description,
            string iconUrl,
            string gamePath,
            string entryFile,
            string developerName,
            string version)
        {
            // 检查游戏对象是否存在
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            // 更新游戏属性
            game.Name = name;
            game.Description = description;
            game.IconUrl = iconUrl;
            game.GamePath = gamePath;
            game.EntryFile = entryFile;
            game.DeveloperName = developerName;
            game.Version = version;

            // 更新最后修改时间
            game.LastModificationTime = DateTime.Now;
        }
    }
}
