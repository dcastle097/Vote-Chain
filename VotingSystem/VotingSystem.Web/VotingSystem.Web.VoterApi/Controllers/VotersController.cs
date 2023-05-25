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

namespace VotingSystem.Web.VoterApi.Controllers
{
    /// <summary>Expose methods to perform voter related actions.</summary>
    [ApiController]
    [Route("[controller]/{id}")]
    public class VotersController : ControllerBase
    {
        private readonly IPollsService  _pollsService;
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
        /// <param name="id">Id of the voter to be created in the ethereum network.</param>
        /// <param name="voter">Password and creation date of the voter to be created in the ethereum network.</param>
        /// <returns>Whether or not the voter was created in the ethereum network</returns>
        [HttpPost("confirm-registration")]
        [ProducesResponseType(typeof(VoterResponseDto), Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<GenericResponseDto<VoterResponseDto>>> ConfirmRegistration(string id, VoterSetPasswordRequestDto voter)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            try
            {
                var response = await _votersService.ConfirmRegistrationAsync(id, voter);
                
                if(response.Data == null)
                    return NotFound(response);
                
                if(response.ResponseCode == ResponseCode.Forbidden)
                    return Forbid(response.Message);

                if (response.ResponseCode == ResponseCode.BadRequest)
                    return BadRequest(response);
                
                return response;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        /// <summary>Check if the user has a valid account.</summary>
        /// <remarks>
        ///     Try and unlock the ethereum user account, if it works, then the user has a valid account in the system, otherwise, it will return an error.
        /// </remarks>
        /// <param name="id">Id of the voter to be validated in the ethereum network.</param>
        /// <param name="password">Password of the voter in the ethereum network.</param>
        /// <returns>Whether or not the voter has a valid account in the ethereum network</returns>
        [HttpPost("check-credentials")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> Login(string id, [FromForm]string password)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();
            
            try
            {
                var response = await _votersService.CheckCredentialsAsync(id, password);
                
                if (response.ResponseCode == ResponseCode.Unauthorized)
                    return Unauthorized(response);
                
                if(response.ResponseCode == ResponseCode.Ok)
                    return Ok(response);

                return BadRequest(response);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }
        
        /// <summary>List polls.</summary>
        /// <remarks>Get a list of polls for a given voter.</remarks>
        /// <response code="200">List of polls.</response>
        /// <param name="id">Id of the voter.</param>
        /// <returns>List of polls the voter has access to.</returns>
        [HttpGet("polls")]
        [ProducesResponseType(typeof(IEnumerable<PollResponseDto>), Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PollResponseDto>>> GetPolls(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var response = await _pollsService.GetByVoterIdAsync(id);

            if (response.ResponseCode == ResponseCode.Ok)
                return Ok(response);
            
            if(response.ResponseCode == ResponseCode.NotFound)
                return NotFound(response);
            
            return BadRequest(response);
        }
    }
}