using System.IO;
using System.Threading.Tasks;

namespace VotingSystem.Infrastructure.Services.Interfaces
{
    /// <summary>
    ///     Interface to define required methods to send an email.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        ///     Task to send an email message.
        /// </summary>
        /// <param name="recipient">Email and name of the person receiving the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="templateName">html template name used as a body for the message.</param>
        /// <param name="parameters">List of parameters to be replaced in the string.format for the template.</param>
        /// <param name="attachments">List of attached files to send along the email.</param>
        /// <returns>A task</returns>
        Task SendMailAsync((string email, string name) recipient, string subject, string templateName,
            object[] parameters = null, (string name, Stream stream)[] attachments = null);
    }
}