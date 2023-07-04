using App.Domain.Translations;
using Base.Domain;
using Contracts;

namespace App.Domain;

public class News : DomainEntityIdMetaData
{
    public ICollection<Content> Content { get; set; } = default!;
    
    // TODO : image
    // byte[] ?
}