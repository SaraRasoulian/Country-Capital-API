using Application.Contract.Countries;
using Application.Contract.Countries.SoapResponse;
using Application.Contract.Shared;
using Moq;
using Shouldly;

namespace UnitTests.Countries;

public class CountryApplicationTests
{
    private readonly Mock<ISoapHelper> _soapHelper;
    private readonly CountryApplication _countryApplication;
    public CountryApplicationTests()
    {
        _soapHelper = new Mock<ISoapHelper>();
        _countryApplication = new CountryApplication(_soapHelper.Object);
    }

    [Fact]
    public async Task GetCapitalAsync_WhenSoapResponseIsValid_ReturnsDto()
    {
        // Arrange
        var isoCode = "IR";
        var capital = "Tehran";
        var requestBody = CountrySoapConsts.RequestBody.Replace(CountrySoapConsts.Input, isoCode);
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n  <soap:Body>\r\n    <m:CapitalCityResponse xmlns:m=\"http://www.oorsprong.org/websamples.countryinfo\">\r\n      <m:CapitalCityResult>" +
            capital + "</m:CapitalCityResult>\r\n    </m:CapitalCityResponse>\r\n  </soap:Body>\r\n</soap:Envelope>";

        _soapHelper.Setup(m => m.SendRequestAsync(CountrySoapConsts.Endpoint, CountrySoapConsts.Action, requestBody)).ReturnsAsync(response);

        // Act
        var result = await _countryApplication.GetCapitalAsync(isoCode);

        // Assert
        result.ShouldNotBeNull();
        result.CountryCode.ShouldBe(isoCode);
        result.Capital.ShouldBe(capital);
        _soapHelper.Verify(d => d.SendRequestAsync(CountrySoapConsts.Endpoint, CountrySoapConsts.Action, requestBody), Times.Once);
    }

    [Fact]
    public async Task GetCapitalAsync_WhenSoapResponseIsInvalid_ThrowsException()
    {
        // Arrange
        var isoCode = "IR";
        var requestBody = CountrySoapConsts.RequestBody.Replace(CountrySoapConsts.Input, isoCode);
        var response = "InvalidResponse";

        _soapHelper.Setup(m => m.SendRequestAsync(CountrySoapConsts.Endpoint, CountrySoapConsts.Action, requestBody)).ReturnsAsync(response);

        // Act & Assert
        var exception =
            await Should.ThrowAsync<InvalidOperationException>(async () => await _countryApplication.GetCapitalAsync(isoCode));

        _soapHelper.Verify(d => d.SendRequestAsync(CountrySoapConsts.Endpoint, CountrySoapConsts.Action, requestBody), Times.Once);
    }
}
