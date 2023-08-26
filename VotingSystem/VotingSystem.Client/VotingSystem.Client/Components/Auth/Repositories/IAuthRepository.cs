using System;
using System.Threading.Tasks;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Auth.Repositories
{
    public interface IAuthRepository
    {
        Task<VoterResponseDto> CompleteRegistration(Guid id, long createdAt, string password, string passwordConfirmation);
        Task<bool> UserExists(string id, string password);
    }
}