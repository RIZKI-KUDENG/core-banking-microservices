using Application.UseCase.Transactions.Commands.Queries.GetReferenceNumber;
using FluentValidation;

namespace Application.UseCase.Transactions.Queries.GetReferenceNumber;

public class GetReferenceNumberQueryValidator : AbstractValidator<GetReferenceNumberQuery>
{
    public GetReferenceNumberQueryValidator()
    {
        RuleFor(v => v.ReferenceNumber).NotEmpty().WithMessage("Reference number is required");
    }
}