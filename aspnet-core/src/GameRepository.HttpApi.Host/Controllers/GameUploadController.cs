using GameRepository.Games;
using GameRepository.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

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
        [Consumes("multipart/form-data")]
        public async Task<GameDto> UploadGameAsync(
            string name,
            string description,
            string developerName,
            string version,
            IFormFile icon,
            IFormFile gamePackage)
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


        [HttpPut]
        [Consumes("multipart/form-data")]
        [Authorize(GameRepositoryPermissions.Games.Edit)]
        public async Task<GameDto> UpdateGameAsync(
           Guid id,
           string? name,
           string? description,
           string? developerName,
           string? version,
           IFormFile? icon,
           IFormFile? gamePackage)
        {
            // 从数据库获取游戏信息
            var game = await _gameRepository.GetAsync(id);
            if (game == null)
            {
                throw new UserFriendlyException($"Game with ID {id} not found");
            }

            // 解析当前路径信息
            var iconUrl = game.IconUrl;
            var gamePath = game.GamePath;
            var entryFile = game.EntryFile;

            // 从路径中提取游戏文件夹ID（注意：这个ID可能与游戏实体ID不同）
            var pathSegments = game.GamePath.Split('/');
            var folderIdString = pathSegments[pathSegments.Length - 2];

            var gamesRootPath = Path.Combine(_webHostEnvironment.WebRootPath, "games");
            var gameFolder = Path.Combine(gamesRootPath, folderIdString);

            if (!Directory.Exists(gameFolder))
            {
                throw new UserFriendlyException($"Game folder for game {id} not found");
            }

            // 处理图标更新
            if (icon != null && icon.Length > 0)
            {
                // 验证图标文件类型
                var iconExtension = Path.GetExtension(icon.FileName).ToLower();
                if (iconExtension != ".png" && iconExtension != ".jpg" && iconExtension != ".jpeg")
                {
                    throw new UserFriendlyException("Only PNG or JPG files are allowed for game icon");
                }

                // 备份旧图标（通过重命名）
                var oldIconPath = Path.Combine(gameFolder, Path.GetFileName(game.IconUrl));
                if (System.IO.File.Exists(oldIconPath))
                {
                    var backupIconPath = Path.Combine(gameFolder, $"icon_old_{DateTime.Now.Ticks}{Path.GetExtension(oldIconPath)}");
                    System.IO.File.Move(oldIconPath, backupIconPath);
                }

                // 保存新图标
                var newIconPath = Path.Combine(gameFolder, "icon" + iconExtension);
                using (var iconStream = new FileStream(newIconPath, FileMode.Create))
                {
                    await icon.CopyToAsync(iconStream);
                }

                // 更新图标URL
                iconUrl = $"/games/{folderIdString}/icon{iconExtension}";
            }

            // 处理游戏包更新
            if (gamePackage != null && gamePackage.Length > 0)
            {
                // 验证游戏包文件类型
                var gameExtension = Path.GetExtension(gamePackage.FileName).ToLower();
                if (gameExtension != ".zip")
                {
                    throw new UserFriendlyException("Only ZIP files are allowed for game package");
                }

                // 备份旧游戏文件（通过重命名）
                var oldFilesPath = Path.Combine(gameFolder, "files");
                // 定义变量，确保它始终存在
                string backupFilesPath = Path.Combine(gameFolder, $"files_old_{DateTime.Now.Ticks}");

                if (Directory.Exists(oldFilesPath))
                {
                    Directory.Move(oldFilesPath, backupFilesPath);
                }

                // 保存并解压新游戏包
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
                    // 恢复旧文件
                    if (Directory.Exists(backupFilesPath))
                    {
                        if (Directory.Exists(extractPath))
                        {
                            Directory.Delete(extractPath, true);
                        }
                        Directory.Move(backupFilesPath, oldFilesPath);
                    }
                    throw new UserFriendlyException("No HTML files found in the game package");
                }

                // 计算相对路径
                entryFile = indexHtml.Replace(extractPath, "").TrimStart('/', '\\');
            }

            // 更新游戏信息
            await _gameManager.UpdateAsync(
                game,
                name ?? game.Name,
                description ?? game.Description,
                iconUrl,
                gamePath,
                entryFile,
                developerName ?? game.DeveloperName,
                version ?? game.Version
            );

            await _gameRepository.UpdateAsync(game);

            return ObjectMapper.Map<Game, GameDto>(game);
        }
    }
}
