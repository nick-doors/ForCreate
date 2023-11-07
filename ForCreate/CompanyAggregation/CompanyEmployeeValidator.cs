using FluentValidation;

namespace ForCreate.CompanyAggregation;

public class CompanyEmployeeValidator : AbstractValidator<CompanyEmployee>
{
    public CompanyEmployeeValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .MaximumLength(200)
            .EmailAddress()
            .When(x => x.Id == null);
    }
}