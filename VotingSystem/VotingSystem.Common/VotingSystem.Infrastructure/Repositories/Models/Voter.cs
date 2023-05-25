using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace VotingSystem.Infrastructure.Repositories.Models
{
    /// <summary>Voter entity.</summary>
    [DynamoDBTable("Voters")]
    public class Voter
    {
        /// <summary>Account address.</summary>
        public string Id { get; set; }

        /// <summary>Full name.</summary>
        public string Name { get; set; }

        /// <summary>Contact email address.</summary>
        public string Email { get; set; }

        /// <summary>Department where the voter can vote.</summary>
        public string Department { get; set; }

        /// <summary>City where the voter can vote.</summary>
        public string City { get; set; }

        /// <summary>Locality where the voter can vote.</summary>
        public string Locality { get; set; }

        /// <summary>Table assigned to the voter to perform the vote.</summary>
        public string Table { get; set; }

        /// <summary>When the voter was created.</summary>
        public DateTime Created { get; set; }

        /// <summary>Whether the voter ins active in the system or not.</summary>
        public bool IsActive { get; set; }
    }
}