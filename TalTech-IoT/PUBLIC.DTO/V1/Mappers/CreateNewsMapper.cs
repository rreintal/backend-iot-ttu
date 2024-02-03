using App.Domain;
using AutoMapper;
using Base.DAL;
using Public.DTO.Content;

namespace Public.DTO.V1.Mappers;

public class CreateNewsMapper
{
    public static BLL.DTO.V1.News Map(Public.DTO.V1.PostNewsDto postNews, List<BLL.DTO.V1.ContentType> contentTypes)
    {
        var newsId = Guid.NewGuid();
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);

        var titleContent = ContentHelper.CreateContent(postNews.Title, titleContentType, newsId,
            ContentHelper.EContentHelperEntityType.News);

        var bodyContent = ContentHelper.CreateContent(postNews.Body, bodyContentType, newsId,
            ContentHelper.EContentHelperEntityType.News);
        
        var res = new BLL.DTO.V1.News()
        {
            Content = new List<BLL.DTO.V1.Content>()
            { 
                titleContent, bodyContent
            },
            Author = postNews.Author
        };

        res.TopicAreas = TopicAreaMapper.Map(postNews.TopicAreas);
        res.Image = postNews.Image;

        return res;
    }
}