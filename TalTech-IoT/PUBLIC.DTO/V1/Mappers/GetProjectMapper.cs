using App.Domain;

namespace Public.DTO.V1.Mappers;

public class GetProjectMapper
{
    public static GetProject Map(BLL.DTO.V1.Project entity)
    {
        var result = new GetProject()
        {
            Id = entity.Id,
            Body = entity.GetContentValue(ContentTypes.BODY),
            Title = entity.GetContentValue(ContentTypes.TITLE),
            ProjectManager = entity.ProjectManager,
            ProjectVolume = entity.ProjectVolume,
            IsOngoing = entity.IsOngoing,
            Year = entity.Year,
            CreatedAt = entity.CreatedAt,
            ViewCount = entity.ViewCount
        };

        return result;
    }
}