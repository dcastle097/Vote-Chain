using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using VotingSystem.Contracts.Registration.ContractDefinition;

namespace VotingSystem.Contracts.Registration
{
    public partial class RegistrationService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, RegistrationDeployment registrationDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<RegistrationDeployment>().SendRequestAndWaitForReceiptAsync(registrationDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, RegistrationDeployment registrationDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<RegistrationDeployment>().SendRequestAsync(registrationDeployment);
        }

        public static async Task<RegistrationService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, RegistrationDeployment registrationDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, registrationDeployment, cancellationTokenSource);
            return new RegistrationService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public RegistrationService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddPollRequestAsync(AddPollFunction addPollFunction)
        {
             return ContractHandler.SendRequestAsync(addPollFunction);
        }

        public Task<TransactionReceipt> AddPollRequestAndWaitForReceiptAsync(AddPollFunction addPollFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addPollFunction, cancellationToken);
        }

        public Task<string> AddPollRequestAsync(string pollAddress, string title, BigInteger startDate, BigInteger endDate)
        {
            var addPollFunction = new AddPollFunction();
                addPollFunction.PollAddress = pollAddress;
                addPollFunction.Title = title;
                addPollFunction.StartDate = startDate;
                addPollFunction.EndDate = endDate;
            
             return ContractHandler.SendRequestAsync(addPollFunction);
        }

        public Task<TransactionReceipt> AddPollRequestAndWaitForReceiptAsync(string pollAddress, string title, BigInteger startDate, BigInteger endDate, CancellationTokenSource cancellationToken = null)
        {
            var addPollFunction = new AddPollFunction();
                addPollFunction.PollAddress = pollAddress;
                addPollFunction.Title = title;
                addPollFunction.StartDate = startDate;
                addPollFunction.EndDate = endDate;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addPollFunction, cancellationToken);
        }

        public Task<string> AddPollToVoterRequestAsync(AddPollToVoterFunction addPollToVoterFunction)
        {
             return ContractHandler.SendRequestAsync(addPollToVoterFunction);
        }

        public Task<TransactionReceipt> AddPollToVoterRequestAndWaitForReceiptAsync(AddPollToVoterFunction addPollToVoterFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addPollToVoterFunction, cancellationToken);
        }

        public Task<string> AddPollToVoterRequestAsync(string voterAddress, string pollAddress)
        {
            var addPollToVoterFunction = new AddPollToVoterFunction();
                addPollToVoterFunction.VoterAddress = voterAddress;
                addPollToVoterFunction.PollAddress = pollAddress;
            
             return ContractHandler.SendRequestAsync(addPollToVoterFunction);
        }

        public Task<TransactionReceipt> AddPollToVoterRequestAndWaitForReceiptAsync(string voterAddress, string pollAddress, CancellationTokenSource cancellationToken = null)
        {
            var addPollToVoterFunction = new AddPollToVoterFunction();
                addPollToVoterFunction.VoterAddress = voterAddress;
                addPollToVoterFunction.PollAddress = pollAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addPollToVoterFunction, cancellationToken);
        }

        public Task<string> AddVoterRequestAsync(AddVoterFunction addVoterFunction)
        {
             return ContractHandler.SendRequestAsync(addVoterFunction);
        }

        public Task<TransactionReceipt> AddVoterRequestAndWaitForReceiptAsync(AddVoterFunction addVoterFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addVoterFunction, cancellationToken);
        }

        public Task<string> AddVoterRequestAsync(string voterAddress, string name, string email, string department, string city, string locality)
        {
            var addVoterFunction = new AddVoterFunction();
                addVoterFunction.VoterAddress = voterAddress;
                addVoterFunction.Name = name;
                addVoterFunction.Email = email;
                addVoterFunction.Department = department;
                addVoterFunction.City = city;
                addVoterFunction.Locality = locality;
            
             return ContractHandler.SendRequestAsync(addVoterFunction);
        }

        public Task<TransactionReceipt> AddVoterRequestAndWaitForReceiptAsync(string voterAddress, string name, string email, string department, string city, string locality, CancellationTokenSource cancellationToken = null)
        {
            var addVoterFunction = new AddVoterFunction();
                addVoterFunction.VoterAddress = voterAddress;
                addVoterFunction.Name = name;
                addVoterFunction.Email = email;
                addVoterFunction.Department = department;
                addVoterFunction.City = city;
                addVoterFunction.Locality = locality;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addVoterFunction, cancellationToken);
        }

        public Task<GetVoterPollsOutputDTO> GetVoterPollsQueryAsync(GetVoterPollsFunction getVoterPollsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetVoterPollsFunction, GetVoterPollsOutputDTO>(getVoterPollsFunction, blockParameter);
        }

        public Task<GetVoterPollsOutputDTO> GetVoterPollsQueryAsync(string voter, BlockParameter blockParameter = null)
        {
            var getVoterPollsFunction = new GetVoterPollsFunction();
                getVoterPollsFunction.Voter = voter;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetVoterPollsFunction, GetVoterPollsOutputDTO>(getVoterPollsFunction, blockParameter);
        }

        public Task<string> MarkPollAsVotedRequestAsync(MarkPollAsVotedFunction markPollAsVotedFunction)
        {
             return ContractHandler.SendRequestAsync(markPollAsVotedFunction);
        }

        public Task<TransactionReceipt> MarkPollAsVotedRequestAndWaitForReceiptAsync(MarkPollAsVotedFunction markPollAsVotedFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(markPollAsVotedFunction, cancellationToken);
        }

        public Task<string> MarkPollAsVotedRequestAsync(string voterAddress, string pollAddress)
        {
            var markPollAsVotedFunction = new MarkPollAsVotedFunction();
                markPollAsVotedFunction.VoterAddress = voterAddress;
                markPollAsVotedFunction.PollAddress = pollAddress;
            
             return ContractHandler.SendRequestAsync(markPollAsVotedFunction);
        }

        public Task<TransactionReceipt> MarkPollAsVotedRequestAndWaitForReceiptAsync(string voterAddress, string pollAddress, CancellationTokenSource cancellationToken = null)
        {
            var markPollAsVotedFunction = new MarkPollAsVotedFunction();
                markPollAsVotedFunction.VoterAddress = voterAddress;
                markPollAsVotedFunction.PollAddress = pollAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(markPollAsVotedFunction, cancellationToken);
        }

        public Task<PollsOutputDTO> PollsQueryAsync(PollsFunction pollsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<PollsFunction, PollsOutputDTO>(pollsFunction, blockParameter);
        }

        public Task<PollsOutputDTO> PollsQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var pollsFunction = new PollsFunction();
                pollsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryDeserializingToObjectAsync<PollsFunction, PollsOutputDTO>(pollsFunction, blockParameter);
        }

        public Task<VotersOutputDTO> VotersQueryAsync(VotersFunction votersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<VotersFunction, VotersOutputDTO>(votersFunction, blockParameter);
        }

        public Task<VotersOutputDTO> VotersQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var votersFunction = new VotersFunction();
                votersFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryDeserializingToObjectAsync<VotersFunction, VotersOutputDTO>(votersFunction, blockParameter);
        }
    }
}
