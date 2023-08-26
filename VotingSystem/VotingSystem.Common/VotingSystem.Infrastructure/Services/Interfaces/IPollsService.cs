using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Infrastructure.Services.Interfaces
{
    public interface IPollsService
    {
        Task<GenericResponseDto<IEnumerable<PollResponseDto>>> GetAllAsync();
        Task<GenericResponseDto<PollResponseDto>> GetByIdAsync(string id);
        Task<GenericResponseDto<IEnumerable<PollResponseDto>>> GetByVoterIdAsync(string voterId);
        Task<GenericResponseDto<PollResponseDto>> CreateAsync(PollRequestDto poll);
        Task<GenericResponseDto<bool>> AddRightsToVoteAsync(string pollId, string voterId);
        Task<GenericResponseDto<bool>> CastVoteAsync(string pollId, byte option, string voterId, string voterPassword);
    }
}