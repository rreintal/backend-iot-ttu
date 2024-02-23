using App.Domain;
using Public.DTO.Content;
using Public.DTO.V1.OpenSourceSolution;
using ContentType = BLL.DTO.V1.ContentType;

namespace Public.DTO.V1.Mappers;

public class OpenSourceSolutionMapper
{
    public static BLL.DTO.V1.OpenSourceSolution Map(OpenSourceSolution.OpenSourceSolution data, List<ContentType> contentTypes)
    {
        var openSourceSolutionId = Guid.NewGuid();
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);

        var titleContent = ContentHelper.CreateContent(data.Title, titleContentType, openSourceSolutionId,
            ContentHelper.EContentHelperEntityType.OpenSourceSolution);

        var bodyContent = ContentHelper.CreateContent(data.Body, bodyContentType, openSourceSolutionId,
            ContentHelper.EContentHelperEntityType.OpenSourceSolution);
        
        var res = new BLL.DTO.V1.OpenSourceSolution()
        {
            Content = new List<BLL.DTO.V1.Content>()
            { 
                titleContent, bodyContent
            },
            Link = data.Link,
            Private = data.Private
        };

        return res;
    }
    
    public static BLL.DTO.V1.OpenSourceSolution MapToUpdate(OpenSourceSolution.OpenSourceSolution data, List<ContentType> contentTypes, Guid entityId)
    {
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);

        var titleContent = ContentHelper.CreateContent(data.Title, titleContentType, entityId,
            ContentHelper.EContentHelperEntityType.OpenSourceSolution);

        var bodyContent = ContentHelper.CreateContent(data.Body, bodyContentType, entityId,
            ContentHelper.EContentHelperEntityType.OpenSourceSolution);
        
        var res = new BLL.DTO.V1.OpenSourceSolution()
        {
            Id = data.Id,
            Content = new List<BLL.DTO.V1.Content>()
            { 
                titleContent, bodyContent
            },
            Link = data.Link,
            Private = data.Private
        };

        return res;
    }

    public static Public.DTO.V1.OpenSourceSolution.OpenSourceSolution Map(BLL.DTO.V1.OpenSourceSolution entity)
    {
        return new OpenSourceSolution.OpenSourceSolution()
        {
            Id = entity.Id,
            Link = entity.Link,
            Private = entity.Private,
            CreatedAt = entity.CreatedAt,
            Title = LanguageCulture.ALL_LANGUAGES.Select(languageCulture =>
            {
                return new ContentDto()
                {
                    Culture = languageCulture,
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, languageCulture)
                };
            }).ToList(),
            Body = LanguageCulture.ALL_LANGUAGES.Select(languageCulture =>
            {
                return new ContentDto()
                {
                    Culture = languageCulture,
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.BODY, languageCulture)
                };
            }).ToList()
        };
    }

    public static Public.DTO.V1.OpenSourceSolution.GetOpenSourceSolution Map(BLL.DTO.V1.OpenSourceSolution entity, string languageCulture)
    {
        return new GetOpenSourceSolution()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Private = entity.Private,
            Link = entity.Link,
            Title = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, languageCulture),
            Body = ContentHelper.GetContentValue(entity, ContentTypes.BODY, languageCulture)
        };
    }
}