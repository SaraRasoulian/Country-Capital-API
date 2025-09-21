using System.Xml.Serialization;

namespace Application.Contract.Countries.SoapResponse;

public class CapitalBody
{
    [XmlElement(ElementName = "CapitalCityResponse", Namespace = CountrySoapConsts.CountryInfoNamespace)]
    public CapitalCityResponse? CapitalCityResponse { get; set; }
}
