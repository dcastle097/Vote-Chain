using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using VotingSystem.Contracts.Registration;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Responses;
using VotingSystem.Domain.Enums;
using VotingSystem.Domain.Extensions;
using VotingSystem.Infrastructure.Helpers;
using VotingSystem.Infrastructure.Models.Configuration;
using VotingSystem.Infrastructure.Repositories.Interfaces;
using VotingSystem.Infrastructure.Repositories.Models;
using VotingSystem.Infrastructure.Services.Interfaces;

namespace VotingSystem.Infrastructure.Services
{
    public class VotersService : IVotersService
    {
        private readonly EthConfiguration _ethConfiguration;
        private readonly IMailService _mailService;
        private readonly IVotersRepository _votersRepository;
        private readonly Web3 _web3;

        public VotersService(IOptions<EthConfiguration> ethConfiguration, IMailService mailService,
            IVotersRepository votersRepository)
        {
            _ethConfiguration = ethConfiguration.Value;
            _mailService = mailService;
            _votersRepository = votersRepository;
            _web3 = !string.IsNullOrWhiteSpace(_ethConfiguration.OwnerPrivateKey)
                ? new Web3(new Account(_ethConfiguration.OwnerPrivateKey), _ethConfiguration.Url)
                : new Web3(new ManagedAccount(_ethConfiguration.OwnerAccount, _ethConfiguration.OwnerPassword),
                    _ethConfiguration.Url);
            _web3.TransactionManager.UseLegacyAsDefault = true;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<IEnumerable<VoterResponseDto>>> GetAllAsync(bool includeInactive = true)
        {
            var response = new GenericResponseDto<IEnumerable<VoterResponseDto>>();

            var voters = await _votersRepository.GetAsync(includeInactive);
            var enumerable = voters as Voter[] ?? voters.ToArray();

            if (!enumerable.Any())
            {
                response.ResponseCode = ResponseCode.NotFound;
                response.Message = "No voters found";
                return response;
            }

            response.Data = enumerable.Select(v => new VoterResponseDto
            {
                IsActive = v.IsActive,
                City = v.City,
                Created = v.Created,
                Department = v.Department,
                Email = v.Email,
                Id = v.Id,
                Name = v.Name,
                Locality = v.Locality,
                Table = v.Table
            });

            return response;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<VoterResponseDto>> GetByIdAsync(string id)
        {
            var response = new GenericResponseDto<VoterResponseDto>();
            var voter = await _votersRepository.GetByIdAsync(id);

            if (voter == null)
            {
                response.ResponseCode = ResponseCode.NotFound;
                response.Message = $"User with id: {id} doesn't exists";
                return response;
            }

            response.Data = new VoterResponseDto
            {
                IsActive = voter.IsActive,
                City = voter.City,
                Created = voter.Created,
                Department = voter.Department,
                Email = voter.Email,
                Id = voter.Id,
                Name = voter.Name,
                Locality = voter.Locality,
                Table = voter.Table
            };
            response.ResponseCode = ResponseCode.Ok;

            return response;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<VoterResponseDto>> CreateAsync(VoterRequestDto voter)
        {
            var response = new GenericResponseDto<VoterResponseDto>();

            var newVoter = new Voter
            {
                IsActive = false,
                Id = Guid.NewGuid().ToString(),
                City = voter.City,
                Created = DateTime.UtcNow,
                Department = voter.Department,
                Email = voter.Email,
                Locality = voter.Locality,
                Name = voter.Name,
                Table = $"{voter.Table}"
            };

            try
            {
                await _votersRepository.SaveAsync(newVoter);
                await _mailService.SendMailAsync((voter.Email, voter.Name), "Bienvenido", "welcome.html", new object[]
                {
                    QrHelper.Base64QrFromString($"{newVoter.Id};{newVoter.Created.ToUnixTimestamp()}")
                });
                response.ResponseCode = ResponseCode.Created;
                response.Data = new VoterResponseDto
                {
                    IsActive = newVoter.IsActive,
                    City = newVoter.City,
                    Created = newVoter.Created,
                    Department = newVoter.Department,
                    Email = newVoter.Email,
                    Id = newVoter.Id,
                    Name = newVoter.Name,
                    Locality = newVoter.Locality,
                    Table = newVoter.Table
                };
            }
            catch (Exception)
            {
                response.ResponseCode = ResponseCode.BadRequest;
            }

            return response;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<VoterResponseDto>> ConfirmRegistrationAsync(string id,
            VoterActivationRequestDto voter)
        {
            var response = await GetByIdAsync(id);

            if (response.ResponseCode == ResponseCode.NotFound)
                return response;

            if (response.Data.IsActive)
            {
                response.ResponseCode = ResponseCode.Forbidden;
                response.Message = "Voter is already active";
                return response;
            }

            var web3 = new Web3(_ethConfiguration.Url) { TransactionManager = { UseLegacyAsDefault = true } };
            await _web3.Eth.Accounts.SendRequestAsync();
            var newAccount = await web3.Personal.NewAccount.SendRequestAsync(voter.Password);
            await _web3.Eth.GetEtherTransferService()
                .TransferEtherAndWaitForReceiptAsync(newAccount, _ethConfiguration.MinEthBalance);

            var registrationService = new RegistrationService(_web3, _ethConfiguration.RegistrationContractAddress);
            await registrationService.AddVoterRequestAsync(newAccount, response.Data.Name,
                response.Data.Email, response.Data.Department, response.Data.City, response.Data.Locality);

            await _votersRepository.DeleteAsync(id, voter.Created);
            response.Data.Id = newAccount;
            response.Data.IsActive = true;
            await _votersRepository.SaveAsync(new Voter
            {
                IsActive = true,
                City = response.Data.City,
                Created = response.Data.Created,
                Department = response.Data.Department,
                Email = response.Data.Email,
                Id = newAccount,
                Locality = response.Data.Locality,
                Name = response.Data.Name,
                Table = response.Data.Table
            });

            response.Message = "Voter has been confirmed successfully";

            await _mailService.SendMailAsync((response.Data.Email, response.Data.Name), "Identificación",
                "activated.html", new object[]
                {
                    QrHelper.Base64QrFromString(response.Data.Id ?? "")
                });

            return response;
        }

        /// <inheritdoc />
        public async Task<GenericResponseDto<bool>> CheckCredentialsAsync(string id,
            VoterLoginRequestDto voterLoginRequestDto)
        {
            var response = new GenericResponseDto<bool>();
            try
            {
                var web3 = new Web3(_ethConfiguration.Url) { TransactionManager = { UseLegacyAsDefault = true } };
                await web3.Personal.UnlockAccount.SendRequestAsync(id, voterLoginRequestDto.Password, 1);
                response.Data = true;
                response.ResponseCode = ResponseCode.Ok;
                response.Message = "Credentials are valid";
                return response;
            }
            catch (RpcResponseException e)
            {
                switch (e.Message)
                {
                    case "Invalid account or password":
                        response.ResponseCode = ResponseCode.Unauthorized;
                        response.Message = "Invalid account or password";
                        break;
                    case "Account not found":
                        response.ResponseCode = ResponseCode.NotFound;
                        response.Message = "Account not found";
                        break;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            if (response.ResponseCode == ResponseCode.Unauthorized) return response;

            response.ResponseCode = ResponseCode.InternalServerError;
            response.Message = "Internal server error";

            return response;
        }
    }
}