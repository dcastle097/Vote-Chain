using System.Collections.Generic;

namespace VotingSystem.Domain.DTOs.Responses;

/// <summary>Votes casted for a section.</summary>
public class VotesBySectionResponseDto
{
    /// <summary>Type of the section.</summary>
    public SectionType Type { get; set; }
        
    /// <summary>Name of the section.</summary>
    public string Name { get; set; }
        
    /// <summary>Votes casted for the section.</summary>
    public IEnumerable<OptionResponseDto> Options { get; set; }
}