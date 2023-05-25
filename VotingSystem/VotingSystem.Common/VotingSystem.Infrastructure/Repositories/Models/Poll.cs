using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace VotingSystem.Infrastructure.Repositories.Models
{
    /// <summary>
    ///     Definition of a DTO for the polls.
    /// </summary>
    [DynamoDBTable("Polls")]
    public class Poll
    {
        /// <summary>
        ///     Id of the poll contract in the ethereum network.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Title of the poll.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Description statement of the poll.
        /// </summary>
        public string Statement { get; set; }

        /// <summary>
        ///     List of options to select from when voting.
        /// </summary>
        public List<string> Options { get; set; }

        /// <summary>
        ///     List addresses of the allowed voters.
        /// </summary>
        public List<string> Voters { get; set; }

        /// <summary>
        ///     Time when the poll starts allowing votes casting.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Time when the poll stops allowing votes casting.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     Timestamp of when the poll was registered in the platform.
        /// </summary>
        public DateTime Created { get; set; }
    }
}