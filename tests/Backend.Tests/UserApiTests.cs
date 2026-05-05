using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests;

public class UserApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UserApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPoliticians_ShouldReturnUnauthorized_WhenNotLocal()
    {
        var response = await _client.GetAsync("/api/PoliticianData/politicians/V/P2026");
        
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
