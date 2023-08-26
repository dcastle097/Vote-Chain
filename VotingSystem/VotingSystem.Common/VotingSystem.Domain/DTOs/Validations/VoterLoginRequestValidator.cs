using FluentValidation;
using VotingSystem.Domain.DTOs.Requests;

namespace VotingSystem.Domain.DTOs.Validations
{
    public class VoterLoginRequestValidator : AbstractValidator<VoterLoginRequestDto>
    {
        /// <inheritdoc />
        public VoterLoginRequestValidator()
        {
            RuleFor(voterLoginRequestDto => voterLoginRequestDto.Password).NotEmpty().WithMessage("La contraseña es requerida");
            RuleFor(voterLoginRequestDto => voterLoginRequestDto.Password).Length(6, 20)
                .WithMessage("La contraseña debe tener entre 6 y 20 caracteres");
            RuleFor(voterLoginRequestDto => voterLoginRequestDto.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$").WithMessage(
                    "La contraseña debe tener al menos una letra mayúscula, una minúscula, un dígito y un carácter especial");
        }
    }
}