using Application.Contract.Countries.SoapResponse;
using Application.Contract.Shared;
using System.Xml.Serialization;

namespace Application.Contract.Countries;

public class CountryApplication(ISoapHelper soapHelper) : ICountryApplication
{
    public async Task<CountryDto> GetCapitalAsync(string isoCode)
    {
        isoCode = isoCode.Trim().ToUpper();
        var requestBody = CountrySoapConsts.RequestBody.Replace(CountrySoapConsts.Input, isoCode);

        var response = await soapHelper.SendRequestAsync(CountrySoapConsts.Endpoint, CountrySoapConsts.Action, requestBody);

        // Deserialize the Response
        CapitalEnvelope envelope;
        var serializer = new XmlSerializer(typeof(CapitalEnvelope));
        using (var reader = new StringReader(response))
        {
            envelope = (CapitalEnvelope)serializer.Deserialize(reader);
        }

        var capital = envelope?.CapitalBody?.CapitalCityResponse?.CapitalCityResult;

        if (capital is null)
        {
            throw new InvalidOperationException(
                $"SOAP API returned no capital city for country code '{isoCode}'.");
        }

        return new CountryDto
        {
            CountryCode = isoCode,
            Capital = capital.ToString()
        };
    }
}
