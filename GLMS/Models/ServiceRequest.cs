using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Models
{
    // This class stores service request information
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        public int ContractId { get; set; }

        // Navigation property to the related contract
        [ForeignKey("ContractId")]
        public Contract? Contract { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        [Required]
        public string Status { get; set; }
    }
}