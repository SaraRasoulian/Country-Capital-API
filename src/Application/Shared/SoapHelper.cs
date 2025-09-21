using Application.Contract.Shared;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Application.Shared;

public class SoapHelper(IHttpClientFactory httpClientFactory, ILogger<SoapHelper> logger) : ISoapHelper
{
    public async Task<string> SendRequestAsync(string endpoint, string action, string requestBody)
    {
        try
        {
            var client = httpClientFactory.CreateClient("SoapClient");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/xml")
            };

            request.Headers.Add("SOAPAction", action);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("SOAP request to {Endpoint} failed. " +
                "StatusCode: {StatusCode}", endpoint, response.StatusCode);

                throw new HttpRequestException($"SOAP request to {endpoint} failed.");
            }

            logger.LogInformation("SOAP request to {Endpoint} succeeded", endpoint);

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SOAP request to {Endpoint} failed unexpectedly.", endpoint);

            throw new HttpRequestException(
                $"SOAP request to {endpoint} failed. See inner exception for details.", ex);
        }

    }
}
