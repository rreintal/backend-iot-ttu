namespace Public.DTO.V1.Mappers;

public class ReturnNewsMapper
{
    public static Public.DTO.V1.News Map(BLL.DTO.V1.News data, string langCult)
    {
        return new Public.DTO.V1.News()
        {
            Body = data.GetContentValue(langCult, "BODY"),
            CreatedAt = data.CreatedAt,
            Title = data.GetContentValue(langCult, "TITLE"),
            Id = data.Id
        };
        
    }
}