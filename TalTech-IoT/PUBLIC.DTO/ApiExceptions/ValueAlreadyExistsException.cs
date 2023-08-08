namespace Public.DTO.ApiExceptions;

public class ValueAlreadyExistsException : Exception
{
    public string Name { get; set; } = default!;

    public ValueAlreadyExistsException(string? name)
    {
        Name = name;
    }
}