using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Infrastructure.Repositories.Models;

namespace VotingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IPollsRepository
    {
        Task<IEnumerable<Poll>> GetAsync();
        Task<Poll> GetByIdAsync(string id);
        Task AddAsync(Poll poll);
        Task AddVoterAsync(string pollId, string voterId);
    }
}