using App.Domain;
using Project = BLL.DTO.V1.Project;

namespace Public.DTO.V1.Mappers;

public class ProjectAllLangsMapper
{
    public static Public.DTO.V1.ProjectAllLangs Map(Project entity)
    {
        return new ProjectAllLangs()
        {
            Id = entity.Id,
            Year = entity.Year,
            ProjectVolume = entity.ProjectVolume,
            ProjectManager = entity.ProjectManager,
            CreatedAt = entity.CreatedAt,
            Body = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Value = entity.GetContentValue(ContentTypes.BODY, lang),
                    Culture = lang
                };
            }).ToList(),
            Title = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Value = entity.GetContentValue(ContentTypes.TITLE, lang),
                    Culture = lang
                };
            }).ToList()
        };
    }
}