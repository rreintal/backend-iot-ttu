using Base.Domain;

namespace DAL.DTO.V1;

public class News : DomainEntityId
{
    // TODO - kasuta DAL dto ka!!!!
    // TEE REPOS NT ERALDI MEETOD SELLE JAOKS!
    public List<DAL.DTO.V1.Content> Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public List<DAL.DTO.V1.TopicArea> TopicAreas { get; set; } = default!;
}