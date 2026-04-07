using FluentValidation;
using Domain.Entities;

namespace Application.UseCase.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(v => v.CustomerId)
            .GreaterThan(0).WithMessage("CustomerId must be greater than 0.");

        RuleFor(v => v.InitialDeposit)
            .GreaterThanOrEqualTo(50000).WithMessage("InitialDeposit must be greater than or equal to 50000.");

        RuleFor(v => v.Type)
            .IsInEnum().WithMessage("Account type is invalid.");
    }
}

