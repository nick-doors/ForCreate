using FluentValidation;

namespace ForCreate.EmployeeAggregation;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(200)
            .EmailAddress();

        RuleFor(x => x.CompaniesIds)
            .Must((_, c) => !GetIdDuplicates(c).Any())
            .WithMessage((_, c) =>
            {
                var ids = string.Join(", ", GetIdDuplicates(c));
                return $"'{{PropertyName}}' contains non-unique values '{ids}'.";
            });
    }

    private static IEnumerable<Guid> GetIdDuplicates(IEnumerable<Guid> companiesIds)
    {
        return companiesIds
            .GroupBy(x => x)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key);
    }
}