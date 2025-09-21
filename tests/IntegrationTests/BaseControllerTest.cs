namespace IntegrationTests;

public class BaseControllerTest : IClassFixture<IntegrationTestWebApplicationFactory>
{
    protected readonly HttpClient _httpClient;

    public BaseControllerTest(IntegrationTestWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }
}
