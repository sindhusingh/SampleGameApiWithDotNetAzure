using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SampleGameApiWithDotNetAzure.Models
{
    public class Player
    {
        // Cosmos DB requires a top-level "id" field for each document.
        // We're using a GUID to uniquely identify each player.
        // It's generated automatically and is immutable after creation.
        [JsonProperty("id")]
        public string PlayerId { get; private set; } = Guid.NewGuid().ToString();

        // This is the unique identifier provided by PlayFab after user login.
        // It serves as the logical partition key in Cosmos DB for efficient lookups.
        [Required(ErrorMessage = "PlayFabId is required.")]
        public string PlayFabId { get; set; }

        // Optional display name of the player.
        // Max length is limited to avoid excessive data usage and UI issues.
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string? Name { get; set; }

        // Optional but validated email address.
        // Validation ensures well-formed email strings are accepted.
        // While optional now, can be required later for features like email verification.
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        // Represents the player’s current level in the game.
        // Optional but initialized to level 1 by default.
        public int? Level { get; set; } = 1;

        // Timestamp when this player profile was created.
        // Useful for analytics, sorting, or account age tracking.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Cosmos DB uses partition keys to optimize data distribution and query performance.
        // Here we use PlayFabId so that all data for a specific user is stored together.
        [JsonProperty("partitionKey")] // partitionKey string is what we choosed during data base creation
        public string PartitionKey => PlayFabId;
    }
}
