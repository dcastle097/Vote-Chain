namespace VotingSystem.Infrastructure.Models.Configuration
{
    /// <summary>
    ///     Email service configuration parameters
    /// </summary>
    public class MailConfiguration
    {
        /// <summary>
        ///     Port used in the communication with the email service.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     Email address used to send the emails.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     Email address password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Email server address.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     Socket options for connection.
        /// </summary>
        public string SocketOptions { get; set; }

        /// <summary>
        ///     Whether or not the email server requires the use of default credentials.
        /// </summary>
        public bool UseDefaultCredentials { get; set; }
    }
}