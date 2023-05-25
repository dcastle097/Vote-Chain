using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Web.Admin.Models;
using VotingSystem.Web.Admin.Services.Interfaces;

namespace VotingSystem.Web.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IAuthService authService, ILogger<HomeController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginRequestDto loginRequestDto, string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(loginRequestDto);

            if (await _authService.LoginAsync(loginRequestDto.Email, loginRequestDto.Password))
                return RedirectToAction(nameof(Index), nameof(VotersController).Replace("Controller", ""));

            ViewBag.ErrorMessage = "Invalid email or password";
            return View(loginRequestDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}