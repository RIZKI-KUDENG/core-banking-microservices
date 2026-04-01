using FluentValidation;

namespace Application.UseCase.Transactions.Queries.GetTransactionId;


public class GetTransactionIdQueryValidator : AbstractValidator<GetTransactionIdQuery>
{
    public GetTransactionIdQueryValidator()
    {
        RuleFor(v => v.Id).GreaterThan(0).WithMessage("Transaction not found");
    }
}