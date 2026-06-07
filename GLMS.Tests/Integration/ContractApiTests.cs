using Xunit;

namespace GLMS.Tests.Integration
{
    public class ContractApiTests
    {
        [Fact]
        public async Task GetContracts_ReturnsSuccessStatusCode()
        {
            using var client = new HttpClient();

            var response = await client.GetAsync("http://localhost:5190/api/contracts");

            var body = await response.Content.ReadAsStringAsync();

            Assert.True(
                response.IsSuccessStatusCode,
                $"Expected success but got {(int)response.StatusCode} {response.StatusCode}. Body: {body}"
            );
        }
    }
}