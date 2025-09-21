using Application.Contract.Countries.SoapResponse;

namespace Application.Contract.Shared;

public interface ISoapHelper
{
    Task<String> SendRequestAsync(string endpoint, string action, string requestBody);
}
