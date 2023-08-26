using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using VotingSystem.Web.Admin.Models.Configuration;
using VotingSystem.Web.Admin.Services.Interfaces;

namespace VotingSystem.Web.Admin.Services;

public class AuthService : IAuthService
{
    private readonly AdminUserConfiguration _adminUserConfiguration;

    public AuthService(IOptions<AdminUserConfiguration> adminUserConfiguration)
    {
        _adminUserConfiguration = adminUserConfiguration.Value;
    }

    /// <inheritdoc />
    public async Task<bool> LoginAsync(string email, string password)
    {
        if (email != _adminUserConfiguration.Email || password != _adminUserConfiguration.Password)
            return false;

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _adminUserConfiguration.Id.ToString()),
            new Claim(ClaimTypes.Name, _adminUserConfiguration.Name)
        }, CookieAuthenticationDefaults.AuthenticationScheme));

        var context = new HttpContextAccessor().HttpContext;

        if (context == null)
            return false;

        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        return true;
    }

    /// <inheritdoc />
    public async Task LogoutAsync()
    {
        var context = new HttpContextAccessor().HttpContext;

        if (context != null) await context.SignOutAsync();
    }
}