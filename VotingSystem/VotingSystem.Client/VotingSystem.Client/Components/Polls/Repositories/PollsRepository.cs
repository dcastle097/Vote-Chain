using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using VotingSystem.Client.Core.Constants;
using VotingSystem.Client.Core.Services;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Polls.Repositories
{
    public class PollsRepository : WebApiService, IPollsRepository
    {
        private IList<PollResponseDto> _polls;
        
        public async Task<IList<PollResponseDto>> GetPollsAsync(string userId)
        {
            if (_polls != null && _polls.Count > 0) return _polls;
            
            if (!IsConnected) throw new Exception("Device not connected to the internet");
            
            var response = await Api.BaseUrl.AppendPathSegment(string.Format(Api.PollsEndpoint, userId))
                .GetJsonAsync<GenericResponseDto<IEnumerable<PollResponseDto>>>();

            if (response == null) throw new Exception("An error occurred while fetching polls");

            _polls = response.Data.ToList();
            
            return _polls;
        }

        public async Task<PollResponseDto> GetPollDetailAsync(string address)
        {
            if (!IsConnected) throw new Exception("Device not connected to the internet");
            var response = await Api.BaseUrl.AppendPathSegment(string.Format(Api.PollDetailsEndpoint, address)).GetJsonAsync<GenericResponseDto<PollResponseDto>>();
            if (response == null) throw new Exception("An error occurred while fetching poll details");
            return response.Data;
        }

        public async Task<bool> CastVoteAsync(string pollAddress, byte option, string userAddress, string password)
        {
            if (!IsConnected) throw new Exception("Device not connected to the internet");

            var data = new CastVoteDto
            {
                Option = option,
                Password = password,
                VoterId = userAddress
            };
            var response = await Api.BaseUrl.AppendPathSegment(string.Format(Api.CastVoteEndpoint, pollAddress))
                .PostJsonAsync(data)
                .ReceiveJson<GenericResponseDto<bool>>();

            if (response == null) throw new Exception("Login failed");

            return response.Data;
        }
    }
}