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
using VotingSystem.Contracts.Poll.ContractDefinition;

namespace VotingSystem.Contracts.Poll
{
    public partial class PollService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, PollDeployment pollDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<PollDeployment>().SendRequestAndWaitForReceiptAsync(pollDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, PollDeployment pollDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<PollDeployment>().SendRequestAsync(pollDeployment);
        }

        public static async Task<PollService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, PollDeployment pollDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, pollDeployment, cancellationTokenSource);
            return new PollService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public PollService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> EndDateQueryAsync(EndDateFunction endDateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<EndDateFunction, BigInteger>(endDateFunction, blockParameter);
        }

        
        public Task<BigInteger> EndDateQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<EndDateFunction, BigInteger>(null, blockParameter);
        }

        public Task<List<string>> GetOptionsQueryAsync(GetOptionsFunction getOptionsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOptionsFunction, List<string>>(getOptionsFunction, blockParameter);
        }

        
        public Task<List<string>> GetOptionsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOptionsFunction, List<string>>(null, blockParameter);
        }

        public Task<GetResultsOutputDTO> GetResultsQueryAsync(GetResultsFunction getResultsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetResultsFunction, GetResultsOutputDTO>(getResultsFunction, blockParameter);
        }

        public Task<GetResultsOutputDTO> GetResultsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetResultsFunction, GetResultsOutputDTO>(null, blockParameter);
        }

        public Task<string> GiveRightToVoteRequestAsync(GiveRightToVoteFunction giveRightToVoteFunction)
        {
             return ContractHandler.SendRequestAsync(giveRightToVoteFunction);
        }

        public Task<TransactionReceipt> GiveRightToVoteRequestAndWaitForReceiptAsync(GiveRightToVoteFunction giveRightToVoteFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRightToVoteFunction, cancellationToken);
        }

        public Task<string> GiveRightToVoteRequestAsync(string voter, string department, string city, string locality)
        {
            var giveRightToVoteFunction = new GiveRightToVoteFunction();
                giveRightToVoteFunction.Voter = voter;
                giveRightToVoteFunction.Department = department;
                giveRightToVoteFunction.City = city;
                giveRightToVoteFunction.Locality = locality;
            
             return ContractHandler.SendRequestAsync(giveRightToVoteFunction);
        }

        public Task<TransactionReceipt> GiveRightToVoteRequestAndWaitForReceiptAsync(string voter, string department, string city, string locality, CancellationTokenSource cancellationToken = null)
        {
            var giveRightToVoteFunction = new GiveRightToVoteFunction();
                giveRightToVoteFunction.Voter = voter;
                giveRightToVoteFunction.Department = department;
                giveRightToVoteFunction.City = city;
                giveRightToVoteFunction.Locality = locality;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRightToVoteFunction, cancellationToken);
        }

        public Task<BigInteger> StartDateQueryAsync(StartDateFunction startDateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StartDateFunction, BigInteger>(startDateFunction, blockParameter);
        }

        
        public Task<BigInteger> StartDateQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StartDateFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> StatementQueryAsync(StatementFunction statementFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StatementFunction, string>(statementFunction, blockParameter);
        }

        
        public Task<string> StatementQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StatementFunction, string>(null, blockParameter);
        }

        public Task<string> TitleQueryAsync(TitleFunction titleFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TitleFunction, string>(titleFunction, blockParameter);
        }

        
        public Task<string> TitleQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TitleFunction, string>(null, blockParameter);
        }

        public Task<string> VoteRequestAsync(VoteFunction voteFunction)
        {
             return ContractHandler.SendRequestAsync(voteFunction);
        }

        public Task<TransactionReceipt> VoteRequestAndWaitForReceiptAsync(VoteFunction voteFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(voteFunction, cancellationToken);
        }

        public Task<string> VoteRequestAsync(byte option)
        {
            var voteFunction = new VoteFunction();
                voteFunction.Option = option;
            
             return ContractHandler.SendRequestAsync(voteFunction);
        }

        public Task<TransactionReceipt> VoteRequestAndWaitForReceiptAsync(byte option, CancellationTokenSource cancellationToken = null)
        {
            var voteFunction = new VoteFunction();
                voteFunction.Option = option;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(voteFunction, cancellationToken);
        }
    }
}
