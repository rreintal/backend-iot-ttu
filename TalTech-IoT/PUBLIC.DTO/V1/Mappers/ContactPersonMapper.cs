using App.Domain;
using Public.DTO.Content;

namespace Public.DTO.V1.Mappers;

public class ContactPersonMapper
{
    public static BLL.DTO.V1.ContactPerson Map(Public.DTO.V1.ContactPerson entity, List<BLL.DTO.V1.ContentType> contentTypes)
    {
        var contactPersonId = Guid.NewGuid();
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);

        var bodyContent = ContentHelper.CreateContent(entity.Body, bodyContentType, contactPersonId,
            ContentHelper.EContentHelperEntityType.ContactPerson);

        var res = new BLL.DTO.V1.ContactPerson()
        {
            Content = new List<BLL.DTO.V1.Content>()
            { 
                bodyContent
            },
            Name = entity.Name
        };
        return res;
    }

    public static BLL.DTO.V1.ContactPerson MapToUpdate(Public.DTO.V1.ContactPerson entity, List<BLL.DTO.V1.ContentType> contentTypes)
    {
        var bodyContentType = contentTypes.First(x => x.Name == ContentTypes.BODY);

        var bodyContent = ContentHelper.CreateContent(entity.Body, bodyContentType, entity.Id,
            ContentHelper.EContentHelperEntityType.ContactPerson);

        return new BLL.DTO.V1.ContactPerson()
        {
            Id = entity.Id,
            Content = new List<BLL.DTO.V1.Content>()
            { 
                bodyContent
            },
            Name = entity.Name
        };
    }

    public static Public.DTO.V1.ContactPerson Map(BLL.DTO.V1.ContactPerson data)
    {
        return new ContactPerson()
        {
            Id = data.Id,
            Name = data.Name,
            Body = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Value = ContentHelper.GetContentValue(data, ContentTypes.BODY, lang),
                    Culture = lang
                };
            }).ToList(),
        };
    }
}