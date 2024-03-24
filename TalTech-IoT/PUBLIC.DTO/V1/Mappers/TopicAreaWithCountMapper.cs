namespace Public.DTO.V1.Mappers;

public class TopicAreaWithCountMapper
{
    public static Public.DTO.V1.TopicAreaWithCount Map(BLL.DTO.V1.TopicAreaWithCount entity)
    {
        return new TopicAreaWithCount()
        {
            Id = entity.Id,
            Count = entity.Count,
            Name = entity.Name,
        };
    }
}