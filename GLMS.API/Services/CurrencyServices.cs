using System.Text.Json;

namespace GLMS.Services
{
    // This service gets the latest USD to ZAR exchange rate
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        // Inject HttpClient so we can call the external API
        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // This method gets the current USD to ZAR rate from the API
        public async Task<decimal> GetUsdToZarRateAsync()
        {
            string url = "https://open.er-api.com/v6/latest/USD";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using JsonDocument document = JsonDocument.Parse(json);

            decimal zarRate = document.RootElement
                .GetProperty("rates")
                .GetProperty("ZAR")
                .GetDecimal();

            return zarRate;
        }

        // This method converts a USD amount to ZAR
        public decimal ConvertUsdToZar(decimal usdAmount, decimal exchangeRate)
        {
            return usdAmount * exchangeRate;
        }
    }
}