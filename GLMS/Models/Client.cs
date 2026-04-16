using System.ComponentModel.DataAnnotations;

namespace GLMS.Models
{
    // This class stores client information
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ContactDetails { get; set; } = string.Empty;

        [Required]
        public string Region { get; set; } = string.Empty;

        // One client can have many contracts
        public List<Contract>? Contracts { get; set; }
    }
}