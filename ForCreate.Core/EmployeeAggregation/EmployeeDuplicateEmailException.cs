using ForCreate.Shared.Exceptions;

namespace ForCreate.Core.EmployeeAggregation;

public class EmployeeDuplicateEmailException : DefaultException
{
    public EmployeeDuplicateEmailException(ICollection<string> emails)
        : base(
            emails.Count == 1
                ? $"A persons with an email '{emails.Single()}' already exists in the system"
                : $"Persons with emails '{string.Join(", ", emails)}' already exists in the system"
        )
    {
    }
}