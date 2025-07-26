using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SampleGameApiWithDotNetAzure.Models;

namespace SampleGameApiWithDotNetAzure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        // In-memory storage for demonstration purposes
        private static readonly List<PlayerScoreDto> Scores = new();

        [HttpPost("submit")]
        public IActionResult SubmitScore([FromBody] PlayerScoreDto playerScore)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Scores.Add(playerScore);
            return Ok(new { message = "Score submitted successfully." });
        }

        [HttpGet("all")]
        public IActionResult GetAllScores()
        {
            return Ok(Scores);
        }

        [HttpGet("{playerId}")]
        public IActionResult GetScoresByPlayerId(string playerId)
        {
            var playerScores = Scores.Where(s => s.PlayerId == playerId).ToList();

            if (!playerScores.Any())
            {
                return NotFound(new { message = $"No scores found for player '{playerId}'." });
            }

            return Ok(playerScores);
        }
    }
}
