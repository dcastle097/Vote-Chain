namespace VotingSystem.Infrastructure.Models.Configuration
{
    /// <summary>
    ///     Ethereum service configuration parameters.
    /// </summary>
    public class EthConfiguration
    {
        /// <summary>
        ///     address of the ethereum server.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Ethereum Account address used to make the calls.
        /// </summary>
        public string OwnerAccount { get; set; }

        /// <summary>
        ///     Ethereum account password
        /// </summary>
        public string OwnerPassword { get; set; }

        /// <summary>
        ///     Private key of the ethereum account used to make the calls
        /// </summary>
        public string OwnerPrivateKey { get; set; }

        /// <summary>
        ///     Minimum ethereum balance to keep on voters wallets
        /// </summary>
        public long MinEthBalance { get; set; }

        /// <summary>
        ///     Contract address of the main registration DApp
        /// </summary>
        public string RegistrationContractAddress { get; set; }
    }
}