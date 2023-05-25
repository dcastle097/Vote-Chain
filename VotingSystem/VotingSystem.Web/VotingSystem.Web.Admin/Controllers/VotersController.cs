using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.Enums;
using VotingSystem.Infrastructure.Services.Interfaces;

namespace VotingSystem.Web.Admin.Controllers
{
    [Authorize]
    public class VotersController : Controller
    {
        private readonly IVotersService _votersService;

        /// <inheritdoc />
        public VotersController(IVotersService votersService)
        {
            _votersService = votersService;
        }

        public async Task<IActionResult> Index([FromQuery] string paginationToken)
        {
            var voters = await _votersService.GetAsync();

            return View(voters);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction(nameof(Index));

            var voter = await _votersService.GetByIdAsync(id);
            if (voter.ResponseCode != ResponseCode.Ok)
                return RedirectToAction(nameof(Index));

            return View(voter.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name, Email, Department, City, Locality, Table")]
            VoterRequestDto voter)
        {
            try
            {
                var response = await _votersService.CreateAsync(voter);
                if (response.ResponseCode != ResponseCode.Created)
                    return View(voter);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}