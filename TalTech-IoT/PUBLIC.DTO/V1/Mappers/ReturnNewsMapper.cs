using App.Domain;

namespace Public.DTO.V1.Mappers;

public class ReturnNewsMapper
{
    public static Public.DTO.V1.News Map(BLL.DTO.V1.News data, bool thumbnail = false)
    {
        var result =  new Public.DTO.V1.News()
        {
            Body = data.GetContentValue(ContentTypes.BODY),
            CreatedAt = data.CreatedAt,
            Title = data.GetContentValue(ContentTypes.TITLE),
            Id = data.Id,
            Author = data.Author,
            TopicAreas = GetTopicAreaMapper.Map(data.TopicAreas),
            Image = data.Image
        };
        // TODO - viga on äkki DOMAIN - BLL mappingus!

        if (thumbnail)
        {
            result.Image = data.ThumbnailImage;
        }

        return result;
    }

    public BLL.DTO.V1.News Map(Public.DTO.V1.News data)
    {
        // TODO!
        throw new NotImplementedException();
    }
    
    
}