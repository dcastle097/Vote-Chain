using System.Threading.Tasks;

namespace VotingSystem.Web.Admin.Services.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
    Task LogoutAsync();
}