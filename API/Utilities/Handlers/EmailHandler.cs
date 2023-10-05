using API.Contracts;
using System.Net.Mail;

namespace API.Utilities.Handlers
{
    public class EmailHandler : IEmailHandler
    {
        private string _server;
        private int _port;
        private string _fromEmailAddress;

        public EmailHandler(string server, int port, string fromEmailAddress)
        {
            _server = server;
            _port = port;
            _fromEmailAddress = fromEmailAddress;
        }

        public void Send(string subject, string body, string toEmail)
        {
            // Struktur Email
            var message = new MailMessage()
            {
                From = new MailAddress(_fromEmailAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            // Build message
            message.To.Add(new MailAddress(toEmail));

            // Kirim ke server
            using var smtpClient = new SmtpClient(_server, _port);
            smtpClient.Send(message);
        }
    }
}
