using Microsoft.AspNetCore.Mvc;
using SampleGameApiWithDotNetAzure.Data;
using SampleGameApiWithDotNetAzure.Models;
using SampleGameApiWithDotNetAzure.Models.DTOs;
using System.Threading.Tasks;

namespace SampleGameApiWithDotNetAzure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _repository;

        public PlayerController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var player = new Player
            {
                PlayFabId = dto.PlayFabId,
                Name = dto.Name,
                Email = dto.Email,
                Level = dto.Level,
            };

            var created = await _repository.AddPlayerAsync(player);
            return Ok(created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(string id)
        {
            var player = await _repository.GetPlayerByIdAsync(id);
            if (player == null) return NotFound();

            return Ok(player);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _repository.GetAllPlayersAsync();
            return Ok(players);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePlayer(string id, [FromBody] PlayerUpdateDto dto)
        {
            var updated = await _repository.UpdatePlayerAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            var deleted = await _repository.DeletePlayerAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent(); // returns HTTP 204
        }

    }
}