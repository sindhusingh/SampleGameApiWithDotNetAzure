using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SampleGameApiWithDotNetAzure.Models.DTOs
{
    public class PlayerCreateDto
    {
        [Required(ErrorMessage = "PlayFabId is required.")]
        public string PlayFabId { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public int? Level { get; set; } = 1;
    }
}