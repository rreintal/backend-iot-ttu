namespace Public.DTO.V1.Mappers;

public class UpdateProjectMapper
{
    public static BLL.DTO.V1.UpdateProject Map(Public.DTO.V1.UpdateProject entity)
    {
        return new BLL.DTO.V1.UpdateProject()
        {
            Id = entity.Id,
            Year = entity.Year,
            ProjectVolume = entity.ProjectVolume,
            ProjectManager = entity.ProjectManager,
            Body = entity.Body.Select(e =>
            {
                return new BLL.DTO.V1.ContentDto()
                {
                    Culture = e.Culture,
                    Value = e.Value
                };
            }).ToList(),
            Title = entity.Title.Select(e =>
            {
                return new BLL.DTO.V1.ContentDto()
                {
                    Culture = e.Culture,
                    Value = e.Value
                };
            }).ToList()
        };
    }
}