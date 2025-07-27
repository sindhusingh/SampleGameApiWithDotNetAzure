using System.ComponentModel.DataAnnotations;

namespace SampleGameApiWithDotNetAzure.Models.DTOs
{
    public class PlayerUpdateDto
    {
        [StringLength(50)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public int? Level { get; set; }
    }
}