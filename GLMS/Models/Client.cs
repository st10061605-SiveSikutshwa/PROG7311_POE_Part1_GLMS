using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace GLMS.Models
{
    // This class stores client information
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactDetails { get; set; }

        [Required]
        public string Region { get; set; }

        // Navigation property:
        // One client can have many contracts
        public List<Contract>? Contracts { get; set; }
    }
}