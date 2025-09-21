namespace Application.Contract.Countries;

public interface ICountryApplication
{
    Task<CountryDto> GetCapitalAsync(string isoCode);
}
