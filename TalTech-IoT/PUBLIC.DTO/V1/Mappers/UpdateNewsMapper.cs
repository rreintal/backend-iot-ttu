using App.Domain;
using Public.DTO.Content;

namespace Public.DTO.V1.Mappers;

public class UpdateNewsMapper
{
    public static BLL.DTO.V1.UpdateNews Map(Public.DTO.V1.UpdateNews entity, List<BLL.DTO.V1.ContentType> contentTypes)
    {
        var entityId = Guid.NewGuid();
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);

        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, entityId,
            ContentHelper.EContentHelperEntityType.HomePageBanner);
        
        var bodyContent = ContentHelper.CreateContent(entity.Body, bodyContentType, entityId,
            ContentHelper.EContentHelperEntityType.HomePageBanner);
        return new BLL.DTO.V1.UpdateNews()
        {
            Id = entity.Id,
            Author = entity.Author,
            Image = entity.Image,
            Content = new List<BLL.DTO.V1.Content>()
            {
                titleContent, bodyContent
            },
            TopicAreas = entity.TopicAreas.Select(ta =>
            {
                return new BLL.DTO.V1.TopicArea()
                {
                    Id = ta.Id
                };
            }).ToList()
            
        };
    }
    
}