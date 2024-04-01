using Base.Domain;

namespace App.DAL.EF.Example;

public class Phone : DomainEntityId
{
    public int Number { get; set; } = default!;
}