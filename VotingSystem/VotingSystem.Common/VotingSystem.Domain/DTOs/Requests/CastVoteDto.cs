namespace VotingSystem.Domain.DTOs.Requests
{
    public class CastVoteDto
    {
        public string VoterId { get; set; }
        public string Password { get; set; }
        public byte Option { get; set; }
    }
}