using FluentValidation;
using VotingSystem.Domain.DTOs.Requests;

namespace VotingSystem.Domain.DTOs.Validations
{
    public class VoterRequestValidator : AbstractValidator<VoterRequestDto>
    {
        /// <inheritdoc />
        public VoterRequestValidator()
        {
            RuleFor(voter => voter.Name).NotEmpty().WithMessage("El nombre es requerido");
            RuleFor(voter => voter.Email).NotEmpty().WithMessage("El correo electrónico es requerido");
            RuleFor(voter => voter.Email).EmailAddress().WithMessage("El correo electrónico no es válido");
            RuleFor(voter => voter.Department).NotEmpty().WithMessage("El departamento es requerido");
            RuleFor(voter => voter.City).NotEmpty().WithMessage("La ciudad es requerido");
            RuleFor(voter => voter.Locality).NotEmpty().WithMessage("La localidad es requerida");
            RuleFor(voter => voter.Table).NotEmpty().WithMessage("El número de mesa es requerido");
        }
    }
}