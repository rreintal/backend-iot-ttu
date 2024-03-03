using Base.Domain;

namespace DAL.DTO.V1;

public class ImageResource : DomainEntityId
{
    public string Link { get; set; } = default!; // https://www.myImageRepository.ee/images/0001-0000-0000-0000.jpg
    public Guid? NewsId { get; set; }
    public News? News { get; set; } = default!;
}