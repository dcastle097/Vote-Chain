using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Polls.Repositories
{
    public interface IPollsRepository
    {
        Task<IList<PollResponseDto>> GetPollsAsync(string userId);
        Task<PollResponseDto> GetPollDetailAsync(string address);
        Task<bool> CastVoteAsync(string pollAddress, byte option, string userAddress, string password);
    }
}