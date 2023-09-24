using BLL.DTO.V1;

namespace Public.DTO.V1.Mappers;

public class UpdateNewsMapper
{
    public static BLL.DTO.V1.UpdateNews Map(Public.DTO.V1.UpdateNews entity)
    {
        return new BLL.DTO.V1.UpdateNews()
        {
            Id = entity.Id,
            Author = entity.Author,
            Image = entity.Image,
            CreatedAt = entity.CreatedAt,
            Body =
                entity.Body.Select(publicDto =>
                {
                    return new BLL.DTO.V1.RawContent()
                    {
                        Culture = publicDto.Culture,
                        Value = publicDto.Value
                    };
                }).ToList(),
            Title = entity.Title.Select(publicDto =>
            {
                return new BLL.DTO.V1.RawContent()
                {
                    Value = publicDto.Value,
                    Culture = publicDto.Culture
                };
            }).ToList(),
            TopicAreas = entity.TopicAreas.Select(ta =>
            {
                return new BLL.DTO.V1.TopicArea()
                {
                    Id = ta.Id
                };
            }).ToList()
            
        };
    }
    
}