namespace ForCreate.Shared.Exceptions;

public class ObjectNotFoundException : DefaultException
{
    public ObjectNotFoundException(string objectName, IEnumerable<Guid> ids)
        : base($"{objectName} with ID's '{string.Join(", ", ids)}' was not found")
    {
    }
}