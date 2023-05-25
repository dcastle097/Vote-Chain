using System;
using System.Linq;
using FluentValidation;
using VotingSystem.Domain.DTOs.Requests;

namespace VotingSystem.Domain.DTOs.Validations
{
    public class PollRequestValidator : AbstractValidator<PollRequestDto>
    {
        /// <inheritdoc />
        public PollRequestValidator()
        {
            RuleFor(poll => poll.Title).NotEmpty().WithMessage("El nombre de la consulta es requerido");
            RuleFor(poll => poll.Statement).NotEmpty().WithMessage("La pregunta de la consulta es requerida");
            RuleFor(poll => poll.StartDate).NotEmpty().WithMessage("La fecha de inicio de la consulta es requerida");
            RuleFor(poll => poll.StartDate).GreaterThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("La fecha de inicio de la consulta debe ser mayor o igual a la fecha actual");
            RuleFor(poll => poll.StartDate).LessThanOrEqualTo(poll => poll.EndDate)
                .WithMessage("La fecha de inicio de la consulta debe ser menor o igual a la fecha de finalización");
            RuleFor(poll => poll.EndDate).NotEmpty()
                .WithMessage("La fecha de finalización de la consulta es requerida");
            RuleFor(poll => poll.EndDate).GreaterThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("La fecha de finalización de la consulta debe ser mayor o igual a la fecha actual");
            RuleFor(poll => poll.EndDate).GreaterThan(poll => poll.StartDate)
                .WithMessage("La fecha de finalización de la consulta debe ser mayor a la fecha de inicio");
            RuleFor(poll => poll.Options).NotEmpty().WithMessage("Las opciones de la consulta son requeridas");
            RuleFor(poll => poll.Options).Must(options => options != null && options.Count > 1)
                .WithMessage("La consulta debe tener al menos dos opciones");
            RuleFor(poll => poll.Options)
                .Must(options => options != null && options.Any(option => !string.IsNullOrWhiteSpace(option)))
                .WithMessage("Las opciones de la consulta no pueden ser texto vacío");
        }
    }
}