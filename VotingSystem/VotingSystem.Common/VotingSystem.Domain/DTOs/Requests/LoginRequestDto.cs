using System.ComponentModel;

namespace VotingSystem.Domain.DTOs.Requests
{
    public class LoginRequestDto
    {
        [DisplayName("Correo electrónico")]
        public string Email { get; set; }
        
        [DisplayName("Contraseña")]
        public string Password { get; set; }
    }
}