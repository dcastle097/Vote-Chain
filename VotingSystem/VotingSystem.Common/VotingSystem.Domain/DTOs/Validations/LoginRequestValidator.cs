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
            RuleFor(loginDto => loginDto.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$")
                .WithMessage(
                    "La contraseña debe tener por lo menos una letra mayúscula, una letra minúscula, un número y un caracter especial");
        }
    }
}