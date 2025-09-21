using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class BaseControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient _httpClient;

    public BaseControllerTest(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }
}
