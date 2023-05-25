using System.ComponentModel;

namespace VotingSystem.Domain.DTOs.Requests
{
    /// <summary>Voter entity.</summary>
    public class VoterRequestDto
    {
        /// <summary>Full name.</summary>
        [DisplayName("Nombre")]
        public string Name { get; set; }

        /// <summary>Contact email address.</summary>
        [DisplayName("Correo electrónico")]
        public string Email { get; set; }

        /// <summary>Department where the voter can vote.</summary>
        [DisplayName("Departamento")]
        public string Department { get; set; }

        /// <summary>City where the voter can vote.</summary>
        [DisplayName("Ciudad")]
        public string City { get; set; }

        /// <summary>Locality where the voter can vote.</summary>
        [DisplayName("Localidad")]
        public string Locality { get; set; }

        /// <summary>Table assigned to the voter to perform the vote.</summary>
        [DisplayName("Mesa")]
        public int Table { get; set; }
    }
}