namespace VotingSystem.Domain.DTOs.Requests
{
    public class VoterGiveRightsToVoteDto
    {
        public string VoterId { get; set; }
        public string Email { get; set; }
    }
}