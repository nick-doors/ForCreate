using FluentValidation;
using ForCreate.Core.Enums;

namespace ForCreate.CompanyAggregation;

public class CompanyValidator : AbstractValidator<Company>
{
    public CompanyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(200);

        RuleFor(x => x.Employees)
            .ForEach(x => x
                .SetValidator(_ => new CompanyEmployeeValidator()));

        RuleFor(x => x.Employees)
            .Must((_, e) => !GetIdDuplicates(e).Any())
            .WithMessage((_, e) =>
            {
                var ids = string.Join(", ", GetIdDuplicates(e));
                return $"'{{PropertyName}}' contains non-unique ID values '{ids}'.";
            });

        RuleFor(x => x.Employees)
            .Must((_, e) => !GetEmailDuplicates(e).Any())
            .WithMessage((_, e) =>
            {
                var emails = string.Join(", ", GetEmailDuplicates(e));
                return $"'{{PropertyName}}' contains non-unique emails '{emails}'.";
            });

        RuleFor(x => x.Employees)
            .Must((_, e) => !GetTitleDuplicates(e).Any())
            .WithMessage((_, e) =>
            {
                var titles = string.Join(", ", GetTitleDuplicates(e));
                return $"'{{PropertyName}}' contains non-unique titles '{titles}'.";
            });
    }

    private static IEnumerable<Guid> GetIdDuplicates(IEnumerable<CompanyEmployee> employees)
    {
        return employees
            .Where(x => x.Id != null)
            .GroupBy(x => x.Id!.Value)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key);
    }

    private static IEnumerable<string> GetEmailDuplicates(IEnumerable<CompanyEmployee> employees)
    {
        return employees
            .Where(x => !string.IsNullOrEmpty(x.Email))
            .GroupBy(x => x.Email!, StringComparer.OrdinalIgnoreCase)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key);
    }

    private static IEnumerable<EmployeeTitle> GetTitleDuplicates(IEnumerable<CompanyEmployee> employees)
    {
        return employees
            .Where(x => x.Title != null)
            .GroupBy(x => x.Title!.Value)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key);
    }
}