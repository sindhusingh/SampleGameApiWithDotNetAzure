using Microsoft.Azure.Cosmos;
using SampleGameApiWithDotNetAzure.Models;
using SampleGameApiWithDotNetAzure.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleGameApiWithDotNetAzure.Data
{
    public class CosmosPlayerRepository : IPlayerRepository
    {
        private readonly Container _container;

        public CosmosPlayerRepository(CosmosClient client, string databaseName, string containerName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task<Player> AddPlayerAsync(Player player)
        {
            var response = await _container.CreateItemAsync(player, new PartitionKey(player.PlayFabId));
            return response.Resource;
        }

        public async Task<Player?> GetPlayerByIdAsync(string playerId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                .WithParameter("@id", playerId);

            var resultSet = _container.GetItemQueryIterator<Player>(query);
            var result = await resultSet.ReadNextAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            var query = _container.GetItemQueryIterator<Player>("SELECT * FROM c");
            var results = new List<Player>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<Player?> UpdatePlayerAsync(string playerId, PlayerUpdateDto updateDto)
        {
            var player = await GetPlayerByIdAsync(playerId);
            if (player == null) return null;

            if (updateDto.Name != null) player.Name = updateDto.Name;
            if (updateDto.Email != null) player.Email = updateDto.Email;
            if (updateDto.Level.HasValue) player.Level = updateDto.Level;

            var response = await _container.ReplaceItemAsync(player, player.PlayerId, new PartitionKey(player.PlayFabId));
            return response.Resource;
        }

        public async Task<bool> DeletePlayerAsync(string playerId)
        {
            var player = await GetPlayerByIdAsync(playerId);
            if (player == null) return false;

            await _container.DeleteItemAsync<Player>(playerId, new PartitionKey(player.PlayFabId));
            return true;
        }

    }
}