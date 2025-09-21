using Application.Contract.Shared;
using System.Xml.Serialization;

namespace Application.Contract.Countries.SoapResponse;

[XmlRoot(ElementName = "Envelope", Namespace = SharedSoapConsts.EnvelopeNamespace)]
public class CapitalEnvelope
{
    [XmlElement(ElementName = "Body", Namespace = SharedSoapConsts.EnvelopeNamespace)]
    public CapitalBody? CapitalBody { get; set; }
}
