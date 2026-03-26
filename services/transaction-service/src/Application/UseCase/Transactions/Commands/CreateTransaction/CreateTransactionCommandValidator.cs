using FluentValidation;
using Domain.Entities;


namespace Application.UseCase.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(v => v.Amount).GreaterThan(25000).WithMessage("Amount must be greater than Rp 25.000 ");
        RuleFor(v => v.SourceAccountId).GreaterThan(0).WithMessage("Source Account did not found");
        RuleFor(v => v.DestinationAccountId).GreaterThan(0).WithMessage("Destination Account did not found");
        RuleFor(v => v.TransactionType).IsInEnum().WithMessage("Transaction type is invalid");
    }
}