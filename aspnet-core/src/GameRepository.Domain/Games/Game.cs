using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace GameRepository.Games
{
    public class Game : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string GamePath { get; set; }
        public string EntryFile { get; set; }
        public GameStatus Status { get; set; }
        public string DeveloperName { get; set; }
        public string Version { get; set; }

        public Game()
        {
            Id = Guid.NewGuid();
            Status = GameStatus.PendingReview;
        }
    }
}
