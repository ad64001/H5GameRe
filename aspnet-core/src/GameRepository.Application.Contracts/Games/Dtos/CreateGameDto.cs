
using System.ComponentModel.DataAnnotations;


namespace GameRepository.Games.Dtos
{
    public class CreateGameDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public string IconUrl { get; set; }
        public string GamePath { get; set; }
        public string EntryFile { get; set; }

        [StringLength(128)]
        public string DeveloperName { get; set; }

        [StringLength(32)]
        public string Version { get; set; }
    }
}
