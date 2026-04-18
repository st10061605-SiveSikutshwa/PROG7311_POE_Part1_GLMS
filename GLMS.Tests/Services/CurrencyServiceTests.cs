using GLMS.Services;
using Xunit;

namespace GLMS.Tests.Services
{
    // These tests check the currency conversion logic
    public class CurrencyServiceTests
    {
        [Fact]
        public void ConvertUsdToZar_ShouldReturnCorrectAmount()
        {
            // Arrange
            var service = new CurrencyService(new HttpClient());
            decimal usdAmount = 100m;
            decimal exchangeRate = 18.50m;

            // Act
            decimal result = service.ConvertUsdToZar(usdAmount, exchangeRate);

            // Assert
            Assert.Equal(1850.00m, result);
        }

        [Fact]
        public void ConvertUsdToZar_WithZeroUsd_ShouldReturnZero()
        {
            // Arrange
            var service = new CurrencyService(new HttpClient());
            decimal usdAmount = 0m;
            decimal exchangeRate = 18.50m;

            // Act
            decimal result = service.ConvertUsdToZar(usdAmount, exchangeRate);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void ConvertUsdToZar_WithDecimalValue_ShouldReturnCorrectAmount()
        {
            // Arrange
            var service = new CurrencyService(new HttpClient());
            decimal usdAmount = 25.5m;
            decimal exchangeRate = 18.20m;

            // Act
            decimal result = service.ConvertUsdToZar(usdAmount, exchangeRate);

            // Assert
            Assert.Equal(464.10m, result);
        }
    }
}