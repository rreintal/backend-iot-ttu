namespace Base.Domain;

public abstract class DomainEntityMetaData
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}