using GLMS.Models;

namespace GLMS.ViewModels
{
    // This view model is used to hold the filter values
    // and the filtered list of contracts for the page
    public class ContractFilterViewModel
    {
        public string? Status { get; set; }

        public DateTime? StartDateFrom { get; set; }

        public DateTime? EndDateTo { get; set; }

        public List<Contract>? Contracts { get; set; }
    }
}