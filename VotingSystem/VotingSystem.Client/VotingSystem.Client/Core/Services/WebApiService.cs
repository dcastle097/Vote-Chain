using System.Net.Http;
using Flurl.Http;
using Flurl.Http.Configuration;
using VotingSystem.Client.Core.Constants;
using Xamarin.Essentials;

namespace VotingSystem.Client.Core.Services
{
    public class WebApiService
    {
        public WebApiService()
        {
            FlurlHttp.ConfigureClient(Api.BaseUrl,
                client => client.Settings.HttpClientFactory = new UntrustedCertClientFactory());
        }

        protected static bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
    }

    public class UntrustedCertClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
            };
        }
    }
}