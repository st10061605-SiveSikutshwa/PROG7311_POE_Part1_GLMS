using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Models
{
    // This class stores service request information linked to a contract
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        public int ContractId { get; set; }

        // Navigation property back to the linked contract
        [ForeignKey("ContractId")]
        public Contract? Contract { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        // This will hold the local cost for now
        // Later we will improve this with the currency API
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}