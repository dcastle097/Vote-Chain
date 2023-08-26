namespace VotingSystem.Domain.DTOs.Requests
{
    public class VoterLoginRequestDto
    {
        /// <summary>Password to unlock user's account in the Ethereum network.</summary>
        public string Password { get; set; }
    }
}