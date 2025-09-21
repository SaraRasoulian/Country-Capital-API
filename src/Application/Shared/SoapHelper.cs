using Application.Contract.Shared;
using System.Text;

namespace Application.Shared;

public class SoapHelper : ISoapHelper
{
    public async Task<String> SendRequestAsync(string endpoint, string action, string requestBody)
    {
        try
        {
            var content = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "text/xml")
                };

                request.Headers.Add("SOAPAction", action);

                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    throw new Exception("Soap API Failed.");

                }
                content = await response.Content.ReadAsStringAsync();
            }

            return content;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            throw new Exception("Soap API Failed. " + ex.Message);
        }
    }
}
