using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;
using VotingSystem.Infrastructure.Services.Interfaces;

namespace VotingSystem.Web.VoterApi.Controllers;

/// <summary>Expose methods to perform poll related actions.</summary>
[ApiController]
[Route("[controller]/{pollId}")]
public class PollsController : ControllerBase
{
    private readonly IPollsService _pollsService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PollsController"/> class.
    /// </summary>
    /// <param name="pollsService">
    ///     Polls service. See <see cref="IPollsService" />.
    /// </param>
    public PollsController(IPollsService pollsService)
    {
        _pollsService = pollsService;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetPoll(string pollId)
    {
        var poll = await _pollsService.GetByIdAsync(pollId);
        return Ok(poll);
    }
    
    [HttpPost("vote")]
    public async Task<ActionResult<GenericResponseDto<bool>>> Vote(string pollId, CastVoteDto castVoteDto)
    {
        var result =
            await _pollsService.CastVoteAsync(pollId, castVoteDto.Option, castVoteDto.VoterId, castVoteDto.Password);
        return Ok(result);
    }
}