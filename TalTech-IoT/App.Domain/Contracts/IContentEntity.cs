using System.Collections;

namespace App.Domain.Contracts;

public interface IContentEntity
{
    public ICollection<Content> Content { get; set; }
}