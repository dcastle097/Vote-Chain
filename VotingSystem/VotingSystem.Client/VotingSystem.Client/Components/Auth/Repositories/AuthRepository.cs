using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using VotingSystem.Client.Core.Constants;
using VotingSystem.Client.Core.Services;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Auth.Repositories
{
    public class AuthRepository : WebApiService, IAuthRepository
    {
        /// <inheritdoc />
        public async Task<VoterResponseDto> CompleteRegistration(Guid id, long createdAt, string password)
        {
            if (!IsConnected) throw new Exception("Device not connected to the internet");
            var response = await Api.BaseUrl.AppendPathSegment(string.Format(Api.RegistrationEndpoint, id))
                .PostJsonAsync(new VoterSetPasswordRequestDto {Created = createdAt, Password = password}).ReceiveJson<VoterResponseDto>();

            if (response == null) throw new Exception("Login failed");

            return response;
        }

        /// <inheritdoc />
        public Task<VoterResponseDto> Login(string id, string password)
        {
            throw new NotImplementedException();
        }
    }
}