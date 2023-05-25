using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VotingSystem.Domain.DTOs.Requests
{
    /// <summary>
    ///     Definition of a DTO for creating polls
    /// </summary>
    public class PollRequestDto
    {
        /// <summary>
        ///     Title of the poll.
        /// </summary>
        [DisplayName("Nombre")]
        public string Title { get; set; }

        /// <summary>
        ///     Description statement of the poll.
        /// </summary>
        [DisplayName("Descripción")]
        public string Statement { get; set; }

        /// <summary>
        ///     Time when the poll starts allowing votes casting.
        /// </summary>
        [DisplayName("Fecha de inicio")]
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Time when the poll stops allowing votes casting.
        /// </summary>
        [DisplayName("Fecha de fin")]
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     List of options to select from when voting.
        /// </summary>
        [DisplayName("Opciones de voto")]
        public List<string> Options { get; set; }
    }
}