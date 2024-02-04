using App.Domain;
using BLL.DTO.V1;
using Public.DTO.Content;
using ContentType = BLL.DTO.V1.ContentType;
using Project = BLL.DTO.V1.Project;

namespace Public.DTO.V1.Mappers;

public class ProjectMapper
{
    public static Project Map(PostProjectDto entity, List<ContentType> types)
    {
        var projectId = Guid.NewGuid();
        var bodyContentType = types.First(x => x.Name == ContentTypes.BODY);
        var titleContentType = types.First(x => x.Name == ContentTypes.TITLE);
        
        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, projectId,
            ContentHelper.EContentHelperEntityType.Project);

        var bodyContent = ContentHelper.CreateContent(entity.Body, bodyContentType, projectId,
            ContentHelper.EContentHelperEntityType.Project);

        var projectContent = new List<BLL.DTO.V1.Content>()
        {
            bodyContent, titleContent
        };

        var project = new BLL.DTO.V1.Project()
        {
            Content = projectContent,
            ProjectVolume = entity.ProjectVolume,
            ProjectManager = entity.ProjectManager,
            Year = entity.Year
        };

        return project;
    }

    public static Public.DTO.V1.PostProjectSuccessDto? Map(Project? entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new Public.DTO.V1.PostProjectSuccessDto()
        {
            Id = entity.Id,
            ProjectVolume = entity.ProjectVolume,
            ProjectManager = entity.ProjectManager,
            Year = entity.Year,
            Body = LanguageCulture.ALL_LANGUAGES.Select(languageCulture =>
            {
                return new ContentDto()
                {
                    Culture = languageCulture,
                    Value = entity.GetContentValue(ContentTypes.BODY, languageCulture)
                };
            }).ToList(),
            Title = LanguageCulture.ALL_LANGUAGES.Select(languageCulture =>
            {
                return new ContentDto()
                {
                    Culture = languageCulture,
                    Value = entity.GetContentValue(ContentTypes.TITLE, languageCulture)
                };
            }).ToList()

        };
    }
}