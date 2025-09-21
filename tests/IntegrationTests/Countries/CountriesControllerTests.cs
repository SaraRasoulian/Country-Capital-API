using Application.Contract.Countries;
using Shouldly;
using System.Net.Http.Json;

namespace IntegrationTests.Countries;

public class CountriesControllerTests : BaseControllerTest
{
    public CountriesControllerTests(IntegrationTestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetCapitalAsync_WhenIsoCodeIsValid_ReturnsOK()
    {
        // Arrange
        var isoCode = "FR";
        var url = string.Format("/api/v1/countries/{0}/capital", isoCode);

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        var dto = await response.Content.ReadFromJsonAsync<CountryDto>();
        dto.ShouldNotBeNull();
        dto.CountryCode.ShouldBe(isoCode);
        dto.Capital.ShouldBe("Paris");
    }
}
