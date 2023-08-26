using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;
using VotingSystem.Domain.Enums;
using VotingSystem.Infrastructure.Services.Interfaces;

namespace VotingSystem.Web.Admin.Controllers;

[Authorize]
public class PollsController : Controller
{
    private readonly IPollsService _pollsService;
    private readonly IVotersService _votersService;

    public PollsController(IPollsService pollsService, IVotersService votersService)
    {
        _pollsService = pollsService;
        _votersService = votersService;
    }

    public async Task<IActionResult> Index()
    {
        var polls = await _pollsService.GetAllAsync();

        return View(polls);
    }

    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return RedirectToAction(nameof(Index));

        var poll = await _pollsService.GetByIdAsync(id);
        if (poll.ResponseCode != ResponseCode.Ok)
            return RedirectToAction(nameof(Index));

        if (DateTime.UtcNow >= poll.Data.StartDate) return View(poll.Data);
        
        var voters = await _votersService.GetAllAsync(false);
        voters.Data ??= new List<VoterResponseDto>();
        ViewData["Voters"] = voters.Data?.Where(v => v.IsActive).Select(v => new { v.Id, v.Email }).ToList();

        return View(poll.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Title, Statement, StartDate, EndDate, Options")]
        PollRequestDto pollRequestDto)
    {
        var response = await _pollsService.CreateAsync(pollRequestDto);
        if (response.ResponseCode != ResponseCode.Ok)
            return View(pollRequestDto);
        return RedirectToAction(nameof(System.Index));
    }

    [HttpPost("{id}/give-right-to-vote")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GiveRightToVote([FromRoute] string id,
        VoterGiveRightsToVoteDto voterGiveRightsToVoteDto)
    {
        var response = await _pollsService.AddRightsToVoteAsync(id, voterGiveRightsToVoteDto.VoterId);
        if (response.ResponseCode == ResponseCode.Ok)
        {
            TempData["Message"] = $"Se ha habilitado al usuario con correo {voterGiveRightsToVoteDto.Email} para participar en la votación.";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = $"Ha ocurrido un error al habilitar al usuario con correo {voterGiveRightsToVoteDto.Email}, por favor intente más tarde.";
            TempData["MessageType"] = "danger";
        }
        return RedirectToAction(nameof(Details), new { id });
    }
}