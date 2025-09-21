using System.Xml.Serialization;

namespace Application.Contract.Countries.SoapResponse;

public class CapitalCityResponse
{
    [XmlElement(ElementName = "CapitalCityResult", Namespace = CountrySoapConsts.CountryInfoNamespace)]
    public string? CapitalCityResult { get; set; }
}
