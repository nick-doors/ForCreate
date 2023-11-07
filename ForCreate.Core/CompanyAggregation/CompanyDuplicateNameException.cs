using ForCreate.Shared.Exceptions;

namespace ForCreate.Core.CompanyAggregation;

public class CompanyDuplicateNameException : DefaultException
{
    public CompanyDuplicateNameException(string companyName)
        : base($"A company named '{companyName}' already exists in the system")
    {

    }
}