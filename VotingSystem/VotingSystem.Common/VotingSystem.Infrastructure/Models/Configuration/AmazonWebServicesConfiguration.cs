namespace VotingSystem.Infrastructure.Models.Configuration
{
    /// <summary>
    ///     Amazon Web Services configuration parameters.
    /// </summary>
    public class AmazonWebServicesConfiguration
    {
        /// <summary>
        ///     DynamoDb configuration.
        /// </summary>
        public DynamoDbConfiguration DynamoDb { get; set; }
    }

    /// <summary>
    ///     DynamoDb configuration parameters.
    /// </summary>
    public class DynamoDbConfiguration
    {
        /// <summary>
        ///     DynamoDb access key.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        ///     DynamoDb secret key.
        /// </summary>
        public string SecretKey { get; set; }
    }
}