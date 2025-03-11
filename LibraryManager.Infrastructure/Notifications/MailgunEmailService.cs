using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace LibraryManager.Infrastructure.Notifications
{
    public class MailgunEmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _domain;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public MailgunEmailService(IConfiguration configuration)
        {
            _apiKey = configuration["Mailgun:ApiKey"];
            _domain = configuration["Mailgun:Domain"];
            _fromEmail = configuration["Mailgun:FromEmail"];
            _fromName = configuration["Mailgun:FromName"];
        }

        public async Task SendAsync(string email, string subject, string message)
        {
            // Crie as opções do cliente com o autenticador
            var options = new RestClientOptions($"https://api.mailgun.net/v3/{_domain}")
            {
                Authenticator = new HttpBasicAuthenticator("api", _apiKey)
            };

            // Crie o cliente com as opções
            var client = new RestClient(options);

            var request = new RestRequest("messages", Method.Post);
            request.AddParameter("from", $"{_fromName} <{_fromEmail}>");
            request.AddParameter("to", email);
            request.AddParameter("subject", subject);
            request.AddParameter("text", message);

            await client.ExecuteAsync(request);
        }
    }
} 