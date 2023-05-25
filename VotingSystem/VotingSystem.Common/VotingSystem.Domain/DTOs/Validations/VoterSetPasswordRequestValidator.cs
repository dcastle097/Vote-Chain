using FluentValidation;
using VotingSystem.Domain.DTOs.Requests;

namespace VotingSystem.Domain.DTOs.Validations
{
    public class VoterSetPasswordRequestValidator : AbstractValidator<VoterSetPasswordRequestDto>
    {
        /// <inheritdoc />
        public VoterSetPasswordRequestValidator()
        {
            RuleFor(voterActivationDto => voterActivationDto.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(voterActivationDto => voterActivationDto.Password).Length(6, 20)
                .WithMessage("Password must be between 6 and 20 characters");
            RuleFor(voterActivationDto => voterActivationDto.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$").WithMessage(
                    "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character");
            RuleFor(voterActivationDto => voterActivationDto.Password).Equal(v => v.PasswordConfirmation)
                .WithMessage("Passwords do not match");
        }
    }
}