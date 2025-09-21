using Application.Contract.Shared;
using System.Text;

namespace Application.Shared;

public class SoapHelper(IHttpClientFactory httpClientFactory) : ISoapHelper
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
                Console.WriteLine("Error: " + response.StatusCode);
                throw new Exception("Soap API Failed.");
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            throw new Exception("Soap API Failed. " + ex.Message);
        }
    }
}
