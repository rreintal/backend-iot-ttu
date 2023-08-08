using App.Domain;
using BLL.DTO.V1;
using ContentType = BLL.DTO.V1.ContentType;
using Project = BLL.DTO.V1.Project;

namespace Public.DTO.V1.Mappers;

public class ProjectMapper
{
    public static Project Map(PostProjectDto entity, List<ContentType> types)
    {
        var projectId = Guid.NewGuid();
        var contentTypeBody = types.First(x => x.Name == ContentTypes.BODY);
        var contentTypeTitle = types.First(x => x.Name == ContentTypes.TITLE);
        var estTitle = entity.Title.First(x => x.Culture == LanguageCulture.EST);
        var estBody = entity.Body.First(x => x.Culture == LanguageCulture.EST);
        
        var titleLangStrId = Guid.NewGuid();
        var titleLangStr = new BLL.DTO.V1.LanguageString()
        {
            Id = titleLangStrId,
            Value = estTitle.Value
        };

        var titleContent = new BLL.DTO.V1.Content()
        {
            ContentTypeId = contentTypeTitle.Id,
            ProjectId = projectId,
            LanguageStringId = titleLangStrId,
            LanguageString = titleLangStr
        };

        titleLangStr.Content = titleContent;
        
        var bodyLangStrId = Guid.NewGuid();
        var bodyLangStr = new BLL.DTO.V1.LanguageString()
        {
            Id = bodyLangStrId,
            Value = estBody.Value
        };

        var bodyContent = new BLL.DTO.V1.Content()
        {
            ProjectId = projectId,
            ContentTypeId = contentTypeBody.Id,
            LanguageStringId = bodyLangStrId,
            LanguageString = bodyLangStr
        };

        bodyLangStr.Content = bodyContent;

        List<BLL.DTO.V1.LanguageStringTranslation> BodyTranslations = new List<LanguageStringTranslation>();
        foreach (var content in entity.Body)
        {
            var translation = new BLL.DTO.V1.LanguageStringTranslation()
            {
                LanguageStringId = bodyLangStrId,
                LanguageCulture = content.Culture,
                TranslationValue = content.Value
            };
            BodyTranslations.Add(translation);
        }

        List<BLL.DTO.V1.LanguageStringTranslation> TitleTranslations = new List<LanguageStringTranslation>();
        foreach (var content in entity.Title)
        {
            var translation = new BLL.DTO.V1.LanguageStringTranslation()
            {
                LanguageStringId = titleLangStrId,
                LanguageCulture = content.Culture,
                TranslationValue = content.Value
            };
            TitleTranslations.Add(translation);
        }

        bodyLangStr.LanguageStringTranslations = BodyTranslations;
        titleLangStr.LanguageStringTranslations = TitleTranslations;

        var projectContent = new List<BLL.DTO.V1.Content>()
        {
            bodyContent, titleContent
        };

        var project = new BLL.DTO.V1.Project()
        {
            Content = projectContent,
            Image = entity.Image,
            ProjectVolume = entity.ProjectVolume,
            ProjectManager = entity.ProjectManager,
            TopicAreas = TopicAreaMapper.Map(entity.TopicAreas),
            Year = entity.Year
        };

        return project;
    }

    public PostProjectDto? Map(Project? entity)
    {
        throw new NotImplementedException();
    }
}