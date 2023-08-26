using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;
using VotingSystem.Contracts.Poll;
using VotingSystem.Contracts.Poll.ContractDefinition;
using VotingSystem.Contracts.Registration;
using VotingSystem.Contracts.Registration.ContractDefinition;
using VotingSystem.Domain.Extensions;

const string ownerAccount = "0x77910f229d531b779df2fff4d3582f331e0f5987";
const string ownerPassword = "Inicio.123";
const string nodeUrl = "http://3.138.169.139:8545/";

var web3 = //new Web3(new Account("6024fe1ecced2cfa82147cdc9ea21e3cf2db9bd347f94a18d5356d4a86de71a7"), "http://127.0.0.1:7545");
    new Web3(new ManagedAccount(ownerAccount, ownerPassword), nodeUrl);
web3.TransactionManager.UseLegacyAsDefault = true;

/*
var registryContract = await RegistrationService.DeployContractAndGetServiceAsync(web3, new RegistrationDeployment());
Console.WriteLine($"Registration Contract Address: {registryContract.ContractHandler.ContractAddress}");
*/

var registryContract = new RegistrationService(web3, "0x6fbbb08520ff0d4247e3e5544f87bb421d5563f2");
var voters = await registryContract.VotersQueryAsync(string.Empty);
var voterAddress = "0xff277893d2f094b8c8eece3145dc6860812da81a";

await registryContract.AddVoterRequestAsync(voterAddress, "Juan", "darkangel_191@hotmail.com", "Cundinamarca", "Chía", "Centro");

var pollContract = await PollService.DeployContractAndGetServiceAsync(web3, new PollDeployment
{
    Title = "Consulta de prueba",
    Statement = "lorem ipsum dolor sit amet",
    StartDate = DateTime.UtcNow.AddMinutes(2).ToUnixTimestamp(),
    EndDate = DateTime.UtcNow.AddMinutes(4).ToUnixTimestamp(),
    Options = new List<string> { "Si", "No" },
});

var pollAddress = pollContract.ContractHandler.ContractAddress;

await pollContract.GiveRightToVoteRequestAsync(voterAddress, "Cundinamarca", "Chía", "Centro");
await registryContract.AddPollToVoterRequestAsync(voterAddress, pollAddress);

var userPolls = await registryContract.GetVoterPollsQueryAsync(voterAddress);

var userWeb3 = new Web3(new ManagedAccount(voterAddress, "Inicio.123"), nodeUrl);
var userPollService = new PollService(userWeb3, pollAddress);
await Task.Delay(780000);
var votingResponse = await userPollService.VoteRequestAsync(1);

Console.WriteLine("Press any key to exit the program");
Console.ReadLine();