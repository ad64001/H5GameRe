using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRepository.Games
{
    public enum GameStatus
    {
        PendingReview = 0,
        InReview = 1,
        Approved = 2,
        Rejected = 3
    }
}
