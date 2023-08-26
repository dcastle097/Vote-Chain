using FluentValidation;
using VotingSystem.Domain.DTOs.Requests;

namespace VotingSystem.Domain.DTOs.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        /// <inheritdoc />
        public LoginRequestValidator()
        {
            RuleFor(loginDto => loginDto.Email).NotEmpty().WithMessage("El correo electrónico es requerido");
            RuleFor(loginDto => loginDto.Password).NotEmpty().WithMessage("La contrasña es requerida");
            RuleFor(loginDto => loginDto.Password).Length(6, 20)
                .WithMessage("La contraseña debe tener entre 6 y 20 caracteres");
            RuleFor(voterActivationDto => voterActivationDto.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$").WithMessage(
                    "La contraseña debe tener al menos una letra mayúscula, una minúscula, un dígito y un carácter especial");
        }
    }
}