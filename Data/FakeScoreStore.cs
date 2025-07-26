using System.Collections.Generic;
using System.Linq;
using SampleGameApiWithDotNetAzure.Models;

namespace SampleGameApiWithDotNetAzure.Data
{
    public static class FakeScoreStore
    {
        private static readonly List<PlayerScoreDto> Scores = new();

        public static void AddScore(PlayerScoreDto score)
        {
            Scores.Add(score);
        }

        public static IEnumerable<PlayerScoreDto> GetScoresByPlayerId(string playerId)
        {
            return Scores.Where(s => s.PlayerId == playerId);
        }
    }
}
