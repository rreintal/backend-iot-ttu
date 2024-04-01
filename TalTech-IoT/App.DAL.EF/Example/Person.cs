using Base.Domain;

namespace App.DAL.EF.Example;

public class Person : DomainEntityId
{
    public string Name { get; set; } = default!;
    public IEnumerable<Phone>? Phones { get; set; }
}