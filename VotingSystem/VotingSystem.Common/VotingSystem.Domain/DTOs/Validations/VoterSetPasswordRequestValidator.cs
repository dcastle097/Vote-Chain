using FluentValidation;
using VotingSystem.Domain.DTOs.Requests;

namespace VotingSystem.Domain.DTOs.Validations
{
    public class VoterSetPasswordRequestValidator : AbstractValidator<VoterActivationRequestDto>
    {
        /// <inheritdoc />
        public VoterSetPasswordRequestValidator()
        {
            RuleFor(voterActivationDto => voterActivationDto.Password).NotEmpty().WithMessage("La contraseña es requerida");
            RuleFor(voterActivationDto => voterActivationDto.Password).Length(6, 20)
                .WithMessage("La contraseña debe tener entre 6 y 20 caracteres");
            RuleFor(voterActivationDto => voterActivationDto.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$").WithMessage(
                    "La contraseña debe tener al menos una letra mayúscula, una minúscula, un dígito y un carácter especial");
            RuleFor(voterActivationDto => voterActivationDto.Password).Equal(v => v.PasswordConfirmation)
                .WithMessage("La contraseña y la confirmación no coinciden");
        }
    }
}