using System.Collections.Generic;

namespace VotingSystem.Domain.DTOs.Responses;

/// <summary>Results of the poll.</summary>
public class ResultsResponseDto
{
    /// <summary>Number of votes casted for the poll.</summary>
    public int TotalVotes { get; set; }
        
    /// <summary>Number of votes casted for the poll.</summary>
    public IEnumerable<VotesBySectionResponseDto> VotesBySections { get; set; }
        
    /// <summary>Number of votes casted for the poll.</summary>
    public IEnumerable<OptionResponseDto> VotesByOptions { get; set; }
}