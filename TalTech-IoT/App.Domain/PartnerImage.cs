using Base.Domain;

namespace App.Domain;

public class PartnerImage : DomainEntityId
{
    public string? Link { get; set; }
    public string Image { get; set; } = default!;
    public ICollection<ImageResource>? ImageResources { get; set; }
}