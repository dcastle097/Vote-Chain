using System;
using System.Threading.Tasks;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Auth.Repositories
{
    public interface IAuthRepository
    {
        Task<VoterResponseDto> CompleteRegistration(Guid id, long createdAt, string password);
        Task<VoterResponseDto> Login(string id, string password);
    }
}