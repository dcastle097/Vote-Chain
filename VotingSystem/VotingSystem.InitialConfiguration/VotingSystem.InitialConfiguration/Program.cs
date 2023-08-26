using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;
using VotingSystem.Contracts.Registration;
using VotingSystem.Contracts.Registration.ContractDefinition;

const string ownerAccount = "{ACCOUNT_ADDRESS}";
const string ownerPassword = "{ACCOUNT_PASSWORD}";
const string nodeUrl = "{NODE_URL}";

var web3 = new Web3(new ManagedAccount(ownerAccount, ownerPassword), nodeUrl);
web3.TransactionManager.UseLegacyAsDefault = true;


var registryContract = await RegistrationService.DeployContractAndGetServiceAsync(web3, new RegistrationDeployment());
Console.WriteLine($"Registration Contract Address: {registryContract.ContractHandler.ContractAddress}");

Console.WriteLine("Press any key to exit the program");
Console.ReadLine();