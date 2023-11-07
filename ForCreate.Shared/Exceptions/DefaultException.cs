namespace ForCreate.Shared.Exceptions;

public abstract class DefaultException : Exception
{
    protected DefaultException(string message)
        : base(message)
    {
    }
}