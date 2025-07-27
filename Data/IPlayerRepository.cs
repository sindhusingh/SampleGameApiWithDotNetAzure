using System.Collections.Generic;
using System.Threading.Tasks;
using SampleGameApiWithDotNetAzure.Models;
using SampleGameApiWithDotNetAzure.Models.DTOs;

namespace SampleGameApiWithDotNetAzure.Data
{
    public interface IPlayerRepository
    {
        Task<Player> AddPlayerAsync(Player player);
        Task<Player?> GetPlayerByIdAsync(string playerId);
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task<Player?> UpdatePlayerAsync(string playerId, PlayerUpdateDto updateDto);
        Task<bool> DeletePlayerAsync(string playerId);
    }
}