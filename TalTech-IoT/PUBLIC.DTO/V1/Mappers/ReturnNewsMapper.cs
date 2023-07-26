using App.Domain;

namespace Public.DTO.V1.Mappers;

public class ReturnNewsMapper
{
    public static Public.DTO.V1.News Map(BLL.DTO.V1.News data)
    {
        return new Public.DTO.V1.News()
        {
            Body = data.GetContentValue(ContentTypes.BODY),
            CreatedAt = data.CreatedAt,
            Title = data.GetContentValue(ContentTypes.TITLE),
            Id = data.Id,
            Author = data.Author,
            // TODO - ei tagasta parentit, ainult selle spetsiifilise topicu kus on! kas see ok või vaja muuta? mõtle..
            TopicAreas = GetTopicAreaMapper.Map(data.TopicAreas),
            Image = data.Image
        };
    }

    public BLL.DTO.V1.News Map(Public.DTO.V1.News data)
    {
        // TODO!
        throw new NotImplementedException();
    }
    
    
}