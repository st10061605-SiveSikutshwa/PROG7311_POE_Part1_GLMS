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

        // Amount entered by the user in USD
        [Required]
        [Range(0, double.MaxValue)]
        public decimal CostUSD { get; set; }

        // Converted amount saved in ZAR
        [Required]
        [Range(0, double.MaxValue)]
        public decimal CostZAR { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}