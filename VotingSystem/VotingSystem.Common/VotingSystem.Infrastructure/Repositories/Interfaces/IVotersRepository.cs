using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Infrastructure.Repositories.Models;

namespace VotingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IVotersRepository
    {
        Task<IEnumerable<Voter>> GetAsync(bool includeInactive = true);
        Task<Voter> GetByIdAsync(string id);
        Task SaveAsync(Voter voter);
        Task<bool> DeleteAsync(string id, long created);
    }
}