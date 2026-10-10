using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Resend;
using Resend;
namespace Infrastructure.Services;

public class EmailService(IConfiguration configuration)
{
public async Task SendEmail(string email, string code, CancellationToken ct = default)
{
    var apiToken = configuration["ResendApi:ApiKey"];
    if (apiToken == null)
    {
        throw new Exception("Resend API Key not found");
    }
    
    IResend resend = ResendClient.Create( apiToken);
    
    var http = new HttpClient();
    http.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", apiToken);

    var payload = new
    {
        to = new[] { email },
        template = new
        {
            id = "verification-template", // ID lub alias szablonu
            variables = new Dictionary<string, object>
            {
                ["CODE"] = code
            }
        }
    };

   await http.PostAsJsonAsync("https://api.resend.com/emails", payload, cancellationToken: ct);

}
}