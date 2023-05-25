using System;
using System.ComponentModel;

namespace VotingSystem.Domain.DTOs.Responses
{
    /// <summary>Voter entity.</summary>
    public class VoterResponseDto
    {
        /// <summary>Account address.</summary>
        public string Id { get; set; }

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
        public string Table { get; set; }

        /// <summary>When the voter was created.</summary>
        [DisplayName("Fecha de creación")]
        public DateTime Created { get; set; }

        /// <summary>Whether the voter ins active in the system or not.</summary>
        [DisplayName("Activo")]
        public bool IsActive { get; set; }
    }
}