using System.Security.Cryptography.X509Certificates;

namespace Public.DTO.V1.Mappers;

public class TopicAreaMapper
{

    public static List<BLL.DTO.V1.TopicArea> Map(List<Public.DTO.V1.TopicArea> pubTopicAreas)
    {
        var res = new List<BLL.DTO.V1.TopicArea>();
        foreach (var parent in pubTopicAreas)
        {
            var parentDto = new BLL.DTO.V1.TopicArea()
            {
                Id = parent.Id,
            };
            res.Add(parentDto);
        }

        return res;
    }
    
    
    public static List<Public.DTO.V1.TopicArea> Map(List<BLL.DTO.V1.TopicArea> bllTopicAreas)
    {
        var dict = new Dictionary<Guid, Public.DTO.V1.TopicArea>();
        var res = new List<Public.DTO.V1.TopicArea>();
        foreach (var children in bllTopicAreas)
        {
            
            
            
                if (!dict.ContainsKey(children.Id))
                {
                    var parent = new TopicArea()
                    {
                        Id = children.Id,
                        Name = children.GetName()
                    };
                    dict.Add(parent.Id, parent);
                }
                
        }
        
        res.AddRange(dict.Values);

        return res;
    }
}