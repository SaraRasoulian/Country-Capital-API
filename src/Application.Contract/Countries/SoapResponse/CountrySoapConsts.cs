using Application.Contract.Shared;

namespace Application.Contract.Countries.SoapResponse;

public static class CountrySoapConsts
{
    public const string Endpoint = "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso";
    public const string Action = "http://www.oorsprong.org/websamples.countryinfo/CountryInfoService.wso/CapitalCity";
    public const string CountryInfoNamespace = "http://www.oorsprong.org/websamples.countryinfo";
    public const string Input = "Input";
    public const string RequestBody = $@"
    <soap:Envelope 
        xmlns:soap=""{SharedSoapConsts.EnvelopeNamespace}"" 
        xmlns:web=""{CountryInfoNamespace}"">
      <soap:Header/>
      <soap:Body>
        <web:CapitalCity>
          <web:sCountryISOCode>{Input}</web:sCountryISOCode>
        </web:CapitalCity>
      </soap:Body>
    </soap:Envelope>";
}
