using System;
using System.Collections.Generic;

namespace VotingSystem.Domain.DTOs.Responses
{
    /// <summary>Poll entity</summary>
    public class PollResponseDto
    {
        /// <summary>Contract address.</summary>
        public string Address { get; set; }

        /// <summary>Title of the poll.</summary>
        public string Title { get; set; }

        /// <summary>Full statement of the poll.</summary>
        public string Statement { get; set; }

        /// <summary>Number of votes casted for the poll.</summary>
        public int VoteCount { get; set; }

        /// <summary>Time at which the poll begins receiving votes.</summary>
        public DateTime StartDate { get; set; }

        /// <summary>Time at which the poll stops receiving votes.</summary>
        public DateTime EndDate { get; set; }

        /// <summary>Whether or not the user already voted to this poll.</summary>
        public bool Voted { get; set; }

        /// <summary>List of options to cast a vote to in the poll.</summary>
        public IEnumerable<OptionResponseDto> Options { get; set; }
        
        /// <summary>Results of the poll.</summary>
        public ResultsResponseDto Results { get; set; }
    }
}