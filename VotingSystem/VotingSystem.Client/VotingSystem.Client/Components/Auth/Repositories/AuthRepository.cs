using System;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using VotingSystem.Client.Core.Constants;
using VotingSystem.Client.Core.Exceptions;
using VotingSystem.Client.Core.Services;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;
using VotingSystem.Domain.DTOs.Validations;

namespace VotingSystem.Client.Components.Auth.Repositories
{
    public class AuthRepository : WebApiService, IAuthRepository
    {
        /// <inheritdoc />
        public async Task<VoterResponseDto> CompleteRegistration(Guid id, long createdAt, string password, string passwordConfirmation)
        {
            if (!IsConnected) throw new Exception("Device not connected to the internet");
            
            var validator = new VoterSetPasswordRequestValidator();
            var data = new VoterActivationRequestDto
                { Created = createdAt, Password = password, PasswordConfirmation = passwordConfirmation };

            var validationResult = await validator.ValidateAsync(data);
            if(!validationResult.IsValid)
                throw new ValidationException(string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage)));
            
            var response = await Api.BaseUrl.AppendPathSegment(string.Format(Api.RegistrationEndpoint, id))
                .PostJsonAsync(data)
                .ReceiveJson<GenericResponseDto<VoterResponseDto>>();

            if (response == null) throw new Exception("An error occurred while completing registration");

            return response.Data;
        }

        /// <inheritdoc />
        public async Task<bool> UserExists(string id, string password)
        {
            if (!IsConnected) throw new Exception("Device not connected to the internet");
            
            var validator = new VoterLoginRequestValidator();
            var data = new VoterLoginRequestDto { Password = password };
            
            var validationResult = await validator.ValidateAsync(data);
            if(!validationResult.IsValid)
                throw new ValidationException(string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage)));
            
            var response = await Api.BaseUrl.AppendPathSegment(string.Format(Api.LoginEndpoint, id))
                .PostJsonAsync(data)
                .ReceiveJson<GenericResponseDto<bool>>();

            if (response == null) throw new Exception("Login failed");

            return response.Data;
        }
    }
}