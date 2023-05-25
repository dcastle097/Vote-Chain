namespace VotingSystem.Domain.DTOs.Requests
{
    /// <summary>Voter completion entity</summary>
    public class VoterSetPasswordRequestDto
    {
        /// <summary>Epoch timestamp for when the user was created in the system.</summary>
        public long Created { get; set; }

        /// <summary>Password to set to the user's account in the Ethereum network.</summary>
        public string Password { get; set; }

        /// <summary>Password confirmation to set to the user's account in the Ethereum network.</summary>
        public string PasswordConfirmation { get; set; }
    }
}