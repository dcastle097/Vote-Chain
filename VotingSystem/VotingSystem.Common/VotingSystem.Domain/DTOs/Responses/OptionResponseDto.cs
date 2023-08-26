namespace VotingSystem.Domain.DTOs.Responses
{
    /// <summary>Poll option entity.</summary>
    public class OptionResponseDto
    {
        /// <summary>Id of the option.</summary>
        public int Index { get; set; }

        /// <summary>Option statement.</summary>
        public string Value { get; set; }

        /// <summary>Number of votes casted for the option.</summary>
        public int VoteCount { get; set; }
    }
}