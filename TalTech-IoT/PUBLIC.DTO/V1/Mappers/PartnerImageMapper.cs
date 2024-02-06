namespace Public.DTO.V1.Mappers;

public class PartnerImageMapper
{
    public static BLL.DTO.V1.PartnerImage Map(Public.DTO.V1.PartnerImage data)
    {
        return new BLL.DTO.V1.PartnerImage()
        {
            Image = data.Image
        };
    }

    public static PartnerImage Map(BLL.DTO.V1.PartnerImage data)
    {
        return new PartnerImage()
        {
            Id = data.Id,
            Image = data.Image
        };
    }
}