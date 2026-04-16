using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Models
{
    // This class stores contract information
    public class Contract
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        // Navigation property to the related client
        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string ServiceLevel { get; set; }

        // This will store the path to the uploaded signed agreement PDF
        public string? SignedAgreementPath { get; set; }

        // One contract can have many service requests
        public List<ServiceRequest>? ServiceRequests { get; set; }
    }
}