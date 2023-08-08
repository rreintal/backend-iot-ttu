using App.Domain;
using Base.Contracts;
using BLL.DTO.V1;
using Content = App.Domain.Content;
using ContentType = BLL.DTO.V1.ContentType;

namespace Public.DTO.V1.Mappers;

public class ProjectMapper
{
    public static Project? Map(PostProjectDto? entity, List<ContentType> types)
    {
        // TODO! 
        var projectId = Guid.NewGuid();
        var contentTypeBody = types.First(x => x.Name == ContentTypes.BODY);
        var contentTypeTitle = types.First(x => x.Name == ContentTypes.TITLE);

        var estTitleLangStrId = Guid.NewGuid();
        var estTitleLangStr = new LanguageString()
        {
            Id = estTitleLangStrId
        };
        var estTitle = new Content()
        {
            ContentTypeId = contentTypeTitle.Id,
            ProjectId = projectId
        };
        
        throw new NotImplementedException();
    }

    public PostProjectDto? Map(Project? entity)
    {
        throw new NotImplementedException();
    }
}