using App.Domain;
using Azure.Core;

namespace Public.DTO.V1.Mappers;

public class GetProjectMapper
{
    public static GetProject Map(BLL.DTO.V1.Project entity)
    {
        var a = entity;
        
        return new GetProject()
        {
            Id = entity.Id,
            Body = entity.GetContentValue(ContentTypes.BODY),
            Title = entity.GetContentValue(ContentTypes.TITLE),
            Image = entity.Image,
            ProjectManager = entity.ProjectManager,
            ProjectVolume = entity.ProjectVolume,
            Year = entity.Year,
            TopicAreas = GetTopicAreaMapper.Map(entity.TopicAreas),
            CreatedAt = entity.CreatedAt
        };
    }
}