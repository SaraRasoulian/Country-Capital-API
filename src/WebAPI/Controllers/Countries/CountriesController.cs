using Application.Contract.Countries;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Countries;

[Route("api/v1/countries")]
[ApiController]
public class CountriesController(ICountryApplication countryApplication) : ControllerBase
{
    [HttpGet("{isoCode:alpha:length(2)}/capital")]

    public async Task<ActionResult<CountryDto>> GetCapitalAsync(string isoCode)
    {
        var result = await countryApplication.GetCapitalAsync(isoCode);
        return Ok(result);
    }
}
