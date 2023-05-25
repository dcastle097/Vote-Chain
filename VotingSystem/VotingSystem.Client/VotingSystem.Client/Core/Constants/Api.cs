namespace VotingSystem.Client.Core.Constants
{
    public class Api
    {
        public static string BaseUrl { get; } = "https://localhost:6001";
        public static string LoginEndpoint { get; } = "/auth/{0}/login";
        public static string RegistrationEndpoint { get; } = "/auth/{0}/register";
    }
}