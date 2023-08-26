using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;
using VotingSystem.Domain.Enums;
using VotingSystem.Infrastructure.Repositories.Interfaces;
using VotingSystem.Infrastructure.Services.Interfaces;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace VotingSystem.Web.VoterApi.Controllers;

/// <summary>Expose methods to perform voter related actions.</summary>
[ApiController]
[Route("[controller]/{userId}")]
public class VotersController : ControllerBase
{
    private readonly IPollsService _pollsService;
    private readonly IVotersService _votersService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="VotersController" /> class.
    /// </summary>
    /// <param name="pollsService">
    ///     Polls service. See <see cref="IPollsService" />.
    /// </param>
    /// <param name="votersService">
    ///     Voters service. See <see cref="IVotersRepository" />.
    /// </param>
    public VotersController(IPollsService pollsService, IVotersService votersService)
    {
        _pollsService = pollsService;
        _votersService = votersService;
    }

    /// <summary>Complete registration.</summary>
    /// <remarks>
    ///     Create a new voter in the ethereum network to be able to interact with the contracts.
    /// </remarks>
    /// <param name="userId">Id of the voter to be created in the ethereum network.</param>
    /// <param name="voter">Password and creation date of the voter to be created in the ethereum network.</param>
    /// <returns>Whether or not the voter was created in the ethereum network</returns>
    [HttpPost("confirm-registration")]
    [ProducesResponseType(typeof(VoterResponseDto), Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<ActionResult<GenericResponseDto<VoterResponseDto>>> ConfirmRegistration(string userId, VoterActivationRequestDto voter)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return NotFound();

        try
        {
            var response = await _votersService.ConfirmRegistrationAsync(userId, voter);

            if (response.Data == null)
                return NotFound(response);

            if (response.ResponseCode == ResponseCode.Forbidden)
                return Forbid(response.Message);

            if (response.ResponseCode == ResponseCode.BadRequest)
                return BadRequest(response);

            return response;
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    /// <summary>Check if the user has a valid account.</summary>
    /// <remarks>
    ///     Try and unlock the ethereum user account, if it works, then the user has a valid account in the system, otherwise,
    ///     it will return an error.
    /// </remarks>
    /// <param name="userId">Id of the voter to be validated in the ethereum network.</param>
    /// <param name="loginRequestDto">Password of the voter in the ethereum network.</param>
    /// <returns>Whether or not the voter has a valid account in the ethereum network</returns>
    [HttpPost("check-credentials")]
    [ProducesResponseType(typeof(GenericResponseDto<bool>), Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<ActionResult<GenericResponseDto<bool>>> Login(string userId, VoterLoginRequestDto loginRequestDto)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return NotFound();

        try
        {
            var response = await _votersService.CheckCredentialsAsync(userId, loginRequestDto);

            if (response.ResponseCode == ResponseCode.Unauthorized)
                return Unauthorized(response);

            if (response.ResponseCode == ResponseCode.Ok)
                return Ok(response);

            return BadRequest(response);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    /// <summary>List polls.</summary>
    /// <remarks>Get a list of polls for a given voter.</remarks>
    /// <response code="200">List of polls.</response>
    /// <param name="userId">Id of the voter.</param>
    /// <returns>List of polls the voter has access to.</returns>
    [HttpGet("polls")]
    [ProducesResponseType(typeof(GenericResponseDto<IEnumerable<PollResponseDto>>), Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<ActionResult<GenericResponseDto<IEnumerable<PollResponseDto>>>> GetPolls(
        string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return NotFound();

        var response = await _pollsService.GetByVoterIdAsync(userId);

        if (response.ResponseCode == ResponseCode.Ok)
            return Ok(response);

        if (response.ResponseCode == ResponseCode.NotFound)
            return NotFound(response);

        return BadRequest(response);
    }
}