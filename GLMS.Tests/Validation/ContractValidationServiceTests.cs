using GLMS.Models;
using GLMS.Services;
using Xunit;

namespace GLMS.Tests.Validation
{
    // These tests check the service request business rule
    public class ContractValidationServiceTests
    {
        [Fact]
        public void CanCreateServiceRequest_WithActiveContract_ShouldReturnTrue()
        {
            // Arrange
            var service = new ContractValidationService();
            var contract = new Contract
            {
                Status = "Active",
                ServiceLevel = "Premium"
            };

            // Act
            bool result = service.CanCreateServiceRequest(contract);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanCreateServiceRequest_WithExpiredContract_ShouldReturnFalse()
        {
            // Arrange
            var service = new ContractValidationService();
            var contract = new Contract
            {
                Status = "Expired",
                ServiceLevel = "Standard"
            };

            // Act
            bool result = service.CanCreateServiceRequest(contract);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanCreateServiceRequest_WithOnHoldContract_ShouldReturnFalse()
        {
            // Arrange
            var service = new ContractValidationService();
            var contract = new Contract
            {
                Status = "On Hold",
                ServiceLevel = "Gold"
            };

            // Act
            bool result = service.CanCreateServiceRequest(contract);

            // Assert
            Assert.False(result);
        }
    }
}