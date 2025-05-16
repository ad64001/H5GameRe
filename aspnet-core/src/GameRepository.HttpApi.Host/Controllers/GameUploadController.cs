using GameRepository.Games;
using GameRepository.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.IO;
using System.Threading.Tasks;
using System;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using System.Linq;

namespace GameRepository.Controllers
{
    [Route("api/game-repository/games/upload")]
    [Authorize(GameRepositoryPermissions.Games.Create)]
    public class GameUploadController : GameRepositoryController
    {
        private readonly IRepository<Game, Guid> _gameRepository;
        private readonly GameManager _gameManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GameUploadController(
            IRepository<Game, Guid> gameRepository,
            GameManager gameManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _gameRepository = gameRepository;
            _gameManager = gameManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<GameDto> UploadGameAsync(
            [FromForm] string name,
            [FromForm] string description,
            [FromForm] string developerName,
            [FromForm] string version,
            [FromForm] IFormFile icon,
            [FromForm] IFormFile gamePackage)
        {
            if (gamePackage == null || gamePackage.Length == 0)
            {
                throw new UserFriendlyException("Game package is required");
            }

            if (icon == null || icon.Length == 0)
            {
                throw new UserFriendlyException("Game icon is required");
            }

            // 验证文件类型
            var gameExtension = Path.GetExtension(gamePackage.FileName).ToLower();
            if (gameExtension != ".zip")
            {
                throw new UserFriendlyException("Only ZIP files are allowed for game package");
            }

            var iconExtension = Path.GetExtension(icon.FileName).ToLower();
            if (iconExtension != ".png" && iconExtension != ".jpg" && iconExtension != ".jpeg")
            {
                throw new UserFriendlyException("Only PNG or JPG files are allowed for game icon");
            }

            // 创建游戏目录
            var gameId = Guid.NewGuid();
            var gamesRootPath = Path.Combine(_webHostEnvironment.WebRootPath, "games");
            var gameFolder = Path.Combine(gamesRootPath, gameId.ToString());

            if (!Directory.Exists(gamesRootPath))
            {
                Directory.CreateDirectory(gamesRootPath);
            }

            Directory.CreateDirectory(gameFolder);

            // 保存图标
            var iconPath = Path.Combine(gameFolder, "icon" + iconExtension);
            using (var iconStream = new FileStream(iconPath, FileMode.Create))
            {
                await icon.CopyToAsync(iconStream);
            }

            // 保存并解压游戏包
            var packagePath = Path.Combine(gameFolder, "game.zip");
            using (var packageStream = new FileStream(packagePath, FileMode.Create))
            {
                await gamePackage.CopyToAsync(packageStream);
            }

            // 解压文件
            var extractPath = Path.Combine(gameFolder, "files");
            Directory.CreateDirectory(extractPath);
            ZipFile.ExtractToDirectory(packagePath, extractPath);

            // 查找HTML入口文件
            var htmlFiles = Directory.GetFiles(extractPath, "*.html", SearchOption.AllDirectories);
            var indexHtml = htmlFiles.FirstOrDefault(f => Path.GetFileName(f).ToLower() == "index.html") ?? htmlFiles.FirstOrDefault();

            if (indexHtml == null)
            {
                throw new UserFriendlyException("No HTML files found in the game package");
            }

            // 计算相对路径
            var entryFile = indexHtml.Replace(extractPath, "").TrimStart('/', '\\');

            // 创建游戏记录
            var iconUrl = $"/games/{gameId}/icon{iconExtension}";
            var gamePath = $"/games/{gameId}/files";

            var game = await _gameManager.CreateAsync(
                name,
                description,
                iconUrl,
                gamePath,
                entryFile,
                developerName,
                version
            );

            await _gameRepository.InsertAsync(game);

            return ObjectMapper.Map<Game, GameDto>(game);
        }
    }
}
