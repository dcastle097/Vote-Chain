using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Infrastructure.Services.Interfaces
{
    public interface IVotersService
    {
        Task<GenericResponseDto<IEnumerable<VoterResponseDto>>> GetAllAsync(bool includeInactive = true);
        Task<GenericResponseDto<VoterResponseDto>> GetByIdAsync(string id);
        Task<GenericResponseDto<VoterResponseDto>> CreateAsync(VoterRequestDto voter);
        Task<GenericResponseDto<VoterResponseDto>> ConfirmRegistrationAsync(string id, VoterActivationRequestDto voter);
        Task<GenericResponseDto<bool>> CheckCredentialsAsync(string id, VoterLoginRequestDto voterLoginRequestDto);
    }
}