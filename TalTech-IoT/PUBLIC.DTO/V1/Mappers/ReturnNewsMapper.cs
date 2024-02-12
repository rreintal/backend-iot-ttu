
using App.Domain;

namespace Public.DTO.V1.Mappers;

public class ReturnNewsMapper
{
    public static News? Map(BLL.DTO.V1.News? data, bool thumbnail = false)
    {
        
        // TODO: better fix, currently hack 
        var body = data!.Content.Count == 1 ? "" : data.GetContentValue(ContentTypes.BODY); // this means when there is only title, then dont get body
        var result =  new News()
        {
            Body = body,
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

    public BLL.DTO.V1.News Map(News data)
    {
        // TODO!
        throw new NotImplementedException();
    }
    
    
}