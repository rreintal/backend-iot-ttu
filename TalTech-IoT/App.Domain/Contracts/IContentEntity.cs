using System.Collections;

namespace App.Domain.Contracts;

public interface IDomainContentEntity
{
    public ICollection<Content> Content { get; set; }
}