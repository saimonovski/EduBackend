using Microsoft.Extensions.Configuration;
using Resend;
using Resend;
namespace Infrastructure.Services;

public class EmailService(IConfiguration configuration)
{
public async Task SendEmail(string email, string code)
{
    IResend resend = ResendClient.Create( "re_eoL856oq_CG4E2s94fjMx4CxgBG22zif2" );

    var resp = await resend.EmailSendAsync( new EmailMessage()
    {
        From = "onboarding@resend.dev",
        To = "saimonovski069@gmail.com",
        Subject = "DoMat weryfikacja",
        HtmlBody = "<h1> DoMat - Weryfikacja</h1> <br> Już prawie to masz wprowadż następujący kod weryfikacyjny w aplikacji <br> <h2> <code>"+code+"</code>",
    } );

}
}