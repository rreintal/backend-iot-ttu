using App.Domain;
using Public.DTO.Content;
using ContentType = BLL.DTO.V1.ContentType;

namespace Public.DTO.V1.Mappers;

public class UpdateProjectMapper
{
    public static BLL.DTO.V1.UpdateProject Map(Public.DTO.V1.UpdateProject entity, List<ContentType> contentTypes)
    {
        var entityId = entity.Id;
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);
        var titleContentType = contentTypes.First(x => x.Name == ContentTypes.TITLE);
        
        var titleContent = ContentHelper.CreateContent(entity.Title, titleContentType, entityId,
            ContentHelper.EContentHelperEntityType.Project);
        
        var bodyContent = ContentHelper.CreateContent(entity.Body, bodyContentType, entityId,
            ContentHelper.EContentHelperEntityType.Project);
        
        return new BLL.DTO.V1.UpdateProject()
        {
            Id = entity.Id,
            Year = entity.Year,
            ProjectVolume = entity.ProjectVolume,
            ProjectManager = entity.ProjectManager,
            Content =  new List<BLL.DTO.V1.Content>()
            {
                titleContent, bodyContent
            }
        };
        
    }
}