namespace VotingSystem.Client.Core.Constants
{
    public class Api
    {
        public static string BaseUrl { get; } = "https://d0p3xqx0-44391.use2.devtunnels.ms/";
        public static string LoginEndpoint { get; } = "/voters/{0}/check-credentials";
        public static string RegistrationEndpoint { get; } = "/voters/{0}/confirm-registration";
        public static string PollsEndpoint { get; } = "/voters/{0}/polls";
        public static string PollDetailsEndpoint { get; } = "/polls/{0}";
        public static string CastVoteEndpoint { get; } = "/polls/{0}/vote";
    }
}