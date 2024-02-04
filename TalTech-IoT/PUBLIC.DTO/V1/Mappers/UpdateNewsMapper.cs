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
            Body =
                entity.Body.Select(publicDto =>
                {
                    return new BLL.DTO.V1.ContentDto()
                    {
                        Culture = publicDto.Culture,
                        Value = publicDto.Value
                    };
                }).ToList(),
            Title = entity.Title.Select(publicDto =>
            {
                return new BLL.DTO.V1.ContentDto()
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