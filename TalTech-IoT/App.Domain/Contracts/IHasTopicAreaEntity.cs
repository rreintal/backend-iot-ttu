namespace App.Domain.Contracts;

public interface IHasTopicAreaEntity
{
    public ICollection<HasTopicArea> HasTopicAreas { get; set; }
}