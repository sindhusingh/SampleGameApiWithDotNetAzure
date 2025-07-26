namespace SampleGameApiWithDotNetAzure.Models
{
    public class PlayerScoreDto
    {
        public string PlayerId { get; set; }
        public int Score { get; set; }
    }
}

//namespace SampleGameApiWithDotNetAzure.Models
//{
//    // This file contains a comparison between using a 'record' and a 'class' for defining DTOs (Data Transfer Objects)

//    // ✅ Option 1: Using record (immutable, value-based)
//    // Records are best suited for immutable data where you don't need to change values after creation
//    // Records provide built-in value equality (two records with same data are considered equal)
//    // Not ideal if you need model validation attributes (e.g., [Required], [Range])

//    public record PlayerScoreDto(string PlayerId, int Score);


//    // ✅ Option 2: Using class (mutable, standard DTO practice in ASP.NET)
//    // Classes are mutable and work well with ASP.NET Core's model binding and validation
//    // Allows adding validation annotations like [Required], [Range], etc.
//    // Equality is reference-based (unless overridden), which is fine for DTOs
//    //public class PlayerScoreDto
//    //{
//    //    public string PlayerId { get; set; }
//    //    public int Score { get; set; }
//    //}

//    // 🔍 Summary:
//    // -----------------------------
//    // | Feature            | record              | class               |
//    // |--------------------|---------------------|----------------------|
//    // | Syntax             | Concise             | Verbose              |
//    // | Mutability         | Immutable by default| Mutable              |
//    // | Equality           | Value-based         | Reference-based      |
//    // | Validation Support | ❌ Limited           | ✅ Full support       |
//    // | Use Case           | Lightweight, readonly data | Standard for Web API DTOs |
//    // -----------------------------

//    // ✅ Recommendation:
//    // Use 'class' for DTOs in ASP.NET Core APIs to allow flexibility, validation, and compatibility with model binding.

//}
