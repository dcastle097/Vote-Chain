using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Polls.Repositories
{
    public interface IPollsListRepository
    {
        Task<IList<PollResponseDto>> GetPollsAsync();
        Task<PollResponseDto> GetPollDetailAsync(string address);
    }
}