using App.Domain;

namespace Public.DTO.V1.Mappers;

public class GetProjectMapper
{
    public static GetProject Map(BLL.DTO.V1.Project entity, bool thumbnail = false)
    {
        var a = entity;
        var result = new GetProject()
        {
            Id = entity.Id,
            Body = entity.GetContentValue(ContentTypes.BODY),
            Title = entity.GetContentValue(ContentTypes.TITLE),
            Image = entity.Image,
            ProjectManager = entity.ProjectManager,
            ProjectVolume = entity.ProjectVolume,
            Year = entity.Year,
            CreatedAt = entity.CreatedAt
        };
        if (thumbnail)
        {
            result.Image = entity.ThumbnailImage;
        }

        return result;
    }
}