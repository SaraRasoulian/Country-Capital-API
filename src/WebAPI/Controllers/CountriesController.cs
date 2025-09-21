using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Serialization;

namespace WebAPI.Controllers
{
    [Route("api/v1/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [HttpGet("{isoCode:alpha:length(2)}/capital")]

        public async Task<ActionResult<CountryDto>> GetCapitalAsync(string isoCode)
        {

            isoCode = isoCode.Trim().ToUpper();
            var soapEndpoint = "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso";

            var soapEnvelope = $@"
<soap:Envelope 
    xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
    xmlns:web=""http://www.oorsprong.org/websamples.countryinfo"">
  <soap:Header/>
  <soap:Body>
    <web:CapitalCity>
      <web:sCountryISOCode>{isoCode}</web:sCountryISOCode>
    </web:CapitalCity>
  </soap:Body>
</soap:Envelope>

";

            try
            {
                // Call the SOAP API
                var soapApiResp = string.Empty;

                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, soapEndpoint)
                    {
                        Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml")
                    };

                    request.Headers.Add("SOAPAction", "http://www.oorsprong.org/websamples.countryinfo/CountryInfoService.wso/CapitalCity");


                    HttpResponseMessage response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                        return BadRequest();

                    }
                    soapApiResp = await response.Content.ReadAsStringAsync();
                }

                // Deserialize the response
                Envelope envelope = null;

                var serializer = new XmlSerializer(typeof(Envelope));
                using (var reader = new StringReader(soapApiResp))
                {
                    envelope = (Envelope)serializer.Deserialize(reader);
                }

                var myResult = envelope?.Body?.CapitalCityResponse?.CapitalCityResult;

                if (myResult is null)
                {
                    return BadRequest();
                }

                Console.WriteLine($"{myResult} is the capital of {isoCode}");


                var result = new CountryDto
                {
                    CountryCode = isoCode,
                    Capital = myResult.ToString()
                };

                return Ok(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        public class CountryDto
        {
            public string CountryCode { get; set; } = null!;
            public string Capital { get; set; } = null!;
        }


        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }


        public class Body
        {
            [XmlElement(ElementName = "CapitalCityResponse", Namespace = "http://www.oorsprong.org/websamples.countryinfo")]
            public CapitalCityResponse CapitalCityResponse { get; set; }
        }

        public class CapitalCityResponse
        {
            [XmlElement(ElementName = "CapitalCityResult", Namespace = "http://www.oorsprong.org/websamples.countryinfo")]
            public string CapitalCityResult { get; set; }
        }

    }
}
