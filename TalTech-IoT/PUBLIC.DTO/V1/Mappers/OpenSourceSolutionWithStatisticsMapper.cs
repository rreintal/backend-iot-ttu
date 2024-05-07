using App.Domain;
using Public.DTO.Content;
using Public.DTO.V1.OpenSourceSolution;

namespace Public.DTO.V1.Mappers;

public class OpenSourceSolutionWithStatisticsMapper
{
    public static OpenSourceSolutionRequestInfo Map(BLL.DTO.V1.OpenSourceSolution entity, string languageCulture)
    {
        return new OpenSourceSolutionRequestInfo()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Private = entity.Private,
            Link = entity.Link,
            Title = ContentHelper.GetContentValue(entity, ContentTypes.TITLE, languageCulture),
            Body = ContentHelper.GetContentValue(entity, ContentTypes.BODY, languageCulture),
            AccessDetails = entity.AccessDetails.Select(e => AccessDetailsMapper.Map(e)).ToList()
        };
    }
}