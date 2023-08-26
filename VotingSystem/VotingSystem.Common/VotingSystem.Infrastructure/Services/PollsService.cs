using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using VotingSystem.Contracts.Poll;
using VotingSystem.Contracts.Poll.ContractDefinition;
using VotingSystem.Contracts.Registration;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;
using VotingSystem.Domain.Enums;
using VotingSystem.Domain.Extensions;
using VotingSystem.Infrastructure.Models.Configuration;
using VotingSystem.Infrastructure.Repositories.Interfaces;
using VotingSystem.Infrastructure.Repositories.Models;
using VotingSystem.Infrastructure.Services.Interfaces;

namespace VotingSystem.Infrastructure.Services
{
    public class PollsService : IPollsService
    {
        private readonly EthConfiguration _ethConfiguration;
        private readonly IPollsRepository _pollsRepository;
        private readonly IVotersRepository _votersRepository;
        private readonly Web3 _web3;

        public PollsService(IOptions<EthConfiguration> ethConfiguration, IPollsRepository pollsRepository,
            IVotersRepository votersRepository)
        {
            _ethConfiguration = ethConfiguration.Value;
            _pollsRepository = pollsRepository;
            _votersRepository = votersRepository;
            _web3 = !string.IsNullOrWhiteSpace(_ethConfiguration.OwnerPrivateKey)
                ? new Web3(new Account(_ethConfiguration.OwnerPrivateKey), _ethConfiguration.Url)
                : new Web3(new ManagedAccount(_ethConfiguration.OwnerAccount, _ethConfiguration.OwnerPassword),
                    _ethConfiguration.Url);
            _web3.TransactionManager.UseLegacyAsDefault = true;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<IEnumerable<PollResponseDto>>> GetAllAsync()
        {
            var response = new GenericResponseDto<IEnumerable<PollResponseDto>>();

            var polls = await _pollsRepository.GetAsync();

            if (polls == null || !polls.Any())
            {
                response.ResponseCode = ResponseCode.NotFound;
                response.Message = "No polls found";
                return response;
            }

            response.Data = polls.Select(p => new PollResponseDto
            {
                Address = p.Id,
                Title = p.Title,
                Options = p.Options.Select((o, i) => new OptionResponseDto { Value = o, Index = i }),
                Statement = p.Statement,
                EndDate = p.EndDate,
                StartDate = p.StartDate
            });

            return response;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<PollResponseDto>> GetByIdAsync(string id)
        {
            var response = new GenericResponseDto<PollResponseDto>();

            try
            {
                var poll = await _pollsRepository.GetByIdAsync(id);

                if (poll == null)
                {
                    response.ResponseCode = ResponseCode.NotFound;
                    response.Message = $"Poll with id: {id} doesn't exists";
                    return response;
                }

                response.Data = new PollResponseDto
                {
                    Address = poll.Id,
                    Title = poll.Title,
                    Options = poll.Options.Select((o, i) => new OptionResponseDto { Value = o, Index = i }),
                    Statement = poll.Statement,
                    EndDate = poll.EndDate,
                    StartDate = poll.StartDate
                };
                response.ResponseCode = ResponseCode.Ok;

                var pollService = new PollService(_web3, id);
                
                if (poll.EndDate > DateTime.UtcNow)
                    return response;
                
                var results = await pollService.GetResultsQueryAsync();
                Console.WriteLine();
            }
            catch (RpcResponseException)
            {
            }
            catch (Exception)
            {
                response.ResponseCode = ResponseCode.InternalServerError;
            }

            return response;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<IEnumerable<PollResponseDto>>> GetByVoterIdAsync(string voterId)
        {
            var response = new GenericResponseDto<IEnumerable<PollResponseDto>>();

            try
            {
                var registrationService = new RegistrationService(_web3, _ethConfiguration.RegistrationContractAddress);
                var voterPolls = await registrationService.GetVoterPollsQueryAsync(voterId);

                var pollsAddresses = voterPolls.ReturnValue1.Select(p => p.Id.ToLowerInvariant()).ToList();

                if (pollsAddresses.Any())
                {
                    var polls = await _pollsRepository.GetAsync();
                    response.Data = polls.Where(p => pollsAddresses.Contains(p.Id.ToLowerInvariant())).Select(p => new PollResponseDto { Address = p.Id, Title = p.Title, StartDate = p.StartDate, EndDate = p.EndDate });
                    response.ResponseCode = ResponseCode.Ok;
                }
                else
                {
                    response.Message = "No polls found";
                    response.ResponseCode = ResponseCode.NotFound;
                }
            }
            catch (Exception)
            {
                response.ResponseCode = ResponseCode.InternalServerError;
            }

            return response;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<PollResponseDto>> CreateAsync(PollRequestDto poll)
        {
            var response = new GenericResponseDto<PollResponseDto>();

            try
            {
                var pollDeployment = new PollDeployment
                {
                    Options = poll.Options,
                    Statement = poll.Statement,
                    Title = poll.Title,
                    EndDate = poll.EndDate.ToUniversalTime().ToUnixTimestamp(),
                    StartDate = poll.StartDate.ToUniversalTime().ToUnixTimestamp()
                };
                var blockPoll = await PollService.DeployContractAndGetServiceAsync(_web3, pollDeployment);
                var registrationService = new RegistrationService(_web3, _ethConfiguration.RegistrationContractAddress);
                var str = await registrationService.AddPollRequestAsync(blockPoll.ContractHandler.ContractAddress,
                    poll.Title, pollDeployment.StartDate, pollDeployment.EndDate);

                var newPoll = new Poll
                {
                    Id = blockPoll.ContractHandler.ContractAddress,
                    Title = poll.Title,
                    Options = poll.Options,
                    Statement = poll.Statement,
                    StartDate = poll.StartDate,
                    EndDate = poll.EndDate
                };

                await _pollsRepository.AddAsync(newPoll);

                response.Data = new PollResponseDto
                {
                    Address = newPoll.Id,
                    Title = newPoll.Title,
                    Options = newPoll.Options.Select((o, i) => new OptionResponseDto { Value = o, Index = i }),
                    Statement = newPoll.Statement,
                    EndDate = newPoll.EndDate,
                    StartDate = newPoll.StartDate
                };
                response.ResponseCode = ResponseCode.Ok;
            }
            catch (Exception)
            {
                response.ResponseCode = ResponseCode.BadRequest;
            }

            return response;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<bool>> AddRightsToVoteAsync(string pollId, string voterId)
        {
            var response = new GenericResponseDto<bool>();
            if (string.IsNullOrWhiteSpace(pollId))
            {
                response.ResponseCode = ResponseCode.BadRequest;
                response.Message = "The poll id is required";
                return response;
            }

            if (string.IsNullOrWhiteSpace(voterId))
            {
                response.ResponseCode = ResponseCode.NotFound;
                response.Message = "The voter id is required";
                return response;
            }

            var poll = await _pollsRepository.GetByIdAsync(pollId);

            if (poll == null)
            {
                response.ResponseCode = ResponseCode.NotFound;
                response.Message = "Poll not found";
                return response;
            }

            var voter = await _votersRepository.GetByIdAsync(voterId);

            if (voter == null)
            {
                response.ResponseCode = ResponseCode.NotFound;
                response.Message = "Voter not found";
                return response;
            }

            try
            {
                var pollService = new PollService(_web3, pollId);
                await pollService.GiveRightToVoteRequestAsync(voter.Id, voter.Department, voter.City, voter.Locality);
                var registrationService = new RegistrationService(_web3, _ethConfiguration.RegistrationContractAddress);
                await registrationService.AddPollToVoterRequestAsync(voter.Id, pollId);

                poll.Voters ??= new List<string>();

                poll.Voters.Add(voter.Id);
                await _pollsRepository.AddVoterAsync(pollId, voterId);
                response.Data = true;
                response.ResponseCode = ResponseCode.Ok;
            }
            catch (Exception)
            {
                response.ResponseCode = ResponseCode.BadRequest;
                response.Message = "Something went wrong";
            }

            return response;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<bool>> CastVoteAsync(string pollId, byte option, string voterId, string voterPassword)
        {
            var response = new GenericResponseDto<bool>();
            if (string.IsNullOrWhiteSpace(pollId))
            {
                response.ResponseCode = ResponseCode.BadRequest;
                response.Message = "The poll id is required";
                return response;
            }

            if (string.IsNullOrWhiteSpace(voterId))
            {
                response.ResponseCode = ResponseCode.NotFound;
                response.Message = "The voter id is required";
                return response;
            }

            var poll = await _pollsRepository.GetByIdAsync(pollId);

            if (poll == null)
            {
                response.ResponseCode = ResponseCode.NotFound;
                response.Message = "Poll not found";
                return response;
            }

            var voter = await _votersRepository.GetByIdAsync(voterId);

            if (voter == null)
            {
                response.ResponseCode = ResponseCode.NotFound;
                response.Message = "Voter not found";
                return response;
            }
            
            try
            {
                var account = new ManagedAccount(voterId, voterPassword);
                var web3 = new Web3(account, _ethConfiguration.Url);
                var pollService = new PollService(web3, pollId);
                var startDate = await pollService.StartDateQueryAsync();
                var endDate = await pollService.EndDateQueryAsync();
                var test = ((long)startDate).ToDateTime();
                var test2 = ((long)startDate).ToDateTime();
                await pollService.VoteRequestAsync(option);
                var registrationService = new RegistrationService(_web3, _ethConfiguration.RegistrationContractAddress);
                await registrationService.MarkPollAsVotedRequestAsync(voterId, pollId);

                response.Data = true;
                response.ResponseCode = ResponseCode.Ok;
            }
            catch (Exception e)
            {
                response.ResponseCode = ResponseCode.BadRequest;
                response.Message = "Something went wrong";
            }

            return response;
        }
    }
}