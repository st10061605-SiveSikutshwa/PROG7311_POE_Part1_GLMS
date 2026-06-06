using GLMS.Models;

namespace GLMS.Services
{
    // This service contains business rules related to contracts
    public class ContractValidationService
    {
        // Check whether a service request is allowed for the selected contract
        public bool CanCreateServiceRequest(Contract contract)
        {
            // Requests are not allowed if the contract is expired or on hold
            if (contract.Status == "Expired" || contract.Status == "On Hold")
            {
                return false;
            }

            return true;
        }
    }
}