using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using VotingSystem.Infrastructure.Models.Configuration;
using VotingSystem.Infrastructure.Services.Interfaces;

namespace VotingSystem.Infrastructure.Services
{
    // <inheritdoc />
    public class MailService : IMailService
    {
        private readonly SmtpClient _client = new SmtpClient();
        private readonly MailConfiguration _options;
        private readonly string _webRootPath;

        /// <summary>
        ///     Ctor. for the <see cref="MailService" />.
        /// </summary>
        /// <param name="mailConfig">Email configuration values set via Dependency Injection.</param>
        /// <param name="webRootPath">Base path of the application</param>
        public MailService(string webRootPath, IOptions<MailConfiguration> mailConfig)
        {
            _options = mailConfig.Value;
            _webRootPath = webRootPath;
        }

        /// <inheritdoc />
        public async Task SendMailAsync((string email, string name) recipient, string title, string templateName,
            object[] parameters = null,
            (string name, Stream stream)[] attachments = null)
        {
            var filePath = Path.Combine(_webRootPath, "email", templateName);
            var str = new StreamReader(filePath);
            var template = await str.ReadToEndAsync();
            var body = parameters == null ? template : string.Format(template, parameters);
            str.Close();

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(MailboxAddress.Parse(_options.Address));
            mailMessage.To.Add(new MailboxAddress(recipient.name, recipient.email));
            mailMessage.Subject = title;

            var bodyBuilder = new BodyBuilder {HtmlBody = body};

            if (_options.SocketOptions.Equals("starttls", StringComparison.InvariantCultureIgnoreCase))
                await _client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls);

            else if (_options.SocketOptions.Equals("ssl", StringComparison.InvariantCultureIgnoreCase))
                await _client.ConnectAsync(_options.Host, _options.Port, true);

            if (attachments != null)
            {
                foreach (var (name, stream) in attachments)
                    bodyBuilder.Attachments.Add(name, stream);
            }
            
            _client.AuthenticationMechanisms.Remove("XOAUTH2");
            await _client.AuthenticateAsync(_options.Address, _options.Password);

            mailMessage.Body = bodyBuilder.ToMessageBody();
            await _client.SendAsync(mailMessage);
        }
    }
}