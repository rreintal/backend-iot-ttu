using App.Domain;
using Public.DTO.Content;

namespace Public.DTO.V1.Mappers;

public class GetContactPersonsAllLanguagesMapper
{
    public static Public.DTO.V1.ContactPerson Map(BLL.DTO.V1.ContactPerson entity)
    {
        
        return new ContactPerson()
        {
            Id = entity.Id,
            Name = entity.Name,
            Body = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Value = ContentHelper.GetContentValue(entity, ContentTypes.BODY, lang),
                    Culture = lang
                };
            }).ToList(),
        };
    }
}