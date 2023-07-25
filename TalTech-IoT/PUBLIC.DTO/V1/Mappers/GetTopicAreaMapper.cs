namespace Public.DTO.V1.Mappers;

public class GetTopicAreaMapper
{
    public static List<Public.DTO.V1.GetTopicArea> Map(List<BLL.DTO.V1.TopicArea> entity)
    {
        var res = new List<Public.DTO.V1.GetTopicArea>();
        foreach (var bllEntity in entity)
        {
            /*
            if (bllEntity.ParentTopicArea != null)
            {
                var bllParent = bllEntity.ParentTopicArea;
                var dtoParent = new Public.DTO.V1.GetTopicArea()
                {
                    Id = bllParent.Id,
                    Name = bllParent.GetName()
                };
                res.Add(dtoParent);
            }
            */

            var dtoChild = new Public.DTO.V1.GetTopicArea()
            {
                Id = bllEntity.Id,
                Name = bllEntity.GetName()
            };
            res.Add(dtoChild);
        }

        return res;
    }
}