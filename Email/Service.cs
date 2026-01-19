using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace ZenticServer.Email;

public class Service(EmailSettings emailSettings)
{
    public async Task SendCode(string sendToEmail, int code)
    {
        var mail = new MimeMessage();
        mail.From.Add(new MailboxAddress("said", emailSettings.Email));
        mail.Sender = new MailboxAddress("", emailSettings.Email);
        mail.Bcc.Add(MailboxAddress.Parse(sendToEmail));
        mail.Body = new TextPart(TextFormat.Text) 
            {Text = $"Здравствуйте, ваш код для Zentik\n{code}\nОцените выполненную курсовую работу студентов второго курса БГТУ Военмех им. Д.Ф.Устинова"};
        mail.Subject = "ZentikTeam";

        using var client = new SmtpClient();
        try {
            await client.ConnectAsync(
                emailSettings.Host, 
                emailSettings.Port, 
                SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(emailSettings.Email, emailSettings.Password);
            await client.SendAsync(mail);
        }
        finally {
            await client.DisconnectAsync(true);
        }
    }
}