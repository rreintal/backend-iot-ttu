using App.Domain;
using Public.DTO.Content;

namespace Public.DTO.V1.Mappers;

public class GetContactPersonMapper
{
    public static Public.DTO.V1.GetContactPerson Map(BLL.DTO.V1.ContactPerson contactPerson)
    {
        return new GetContactPerson()
        {
            Id = contactPerson.Id,
            Name = contactPerson.Name,
            Body = ContentHelper.GetContentValue(contactPerson, ContentTypes.BODY)
        };
    }
}