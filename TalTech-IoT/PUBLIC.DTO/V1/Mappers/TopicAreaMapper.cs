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
            
            if (parent.ChildrenTopicAreas != null)
            {
                foreach (var child in parent.ChildrenTopicAreas)
                {
                    var childDto = new BLL.DTO.V1.TopicArea()
                    {
                        Id = child.Id,
                        ParentTopicAreaId = parent.Id,
                        ParentTopicArea = parentDto
                    };
                    res.Add(childDto);
                }   
            }
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
            if (children.ParentTopicArea != null)
            {
                var parent = children.ParentTopicArea;
                if (dict.ContainsKey(parent.Id))
                {
                    var childrenDto = new Public.DTO.V1.TopicArea()
                    {
                        Id = children.Id,
                        Name = children.GetName()
                    };
                    var parentDto = dict[parent.Id];
                    
                    // Without this if, it produces a weird bug
                    if (parentDto.ChildrenTopicAreas == null)
                    {
                        parentDto.ChildrenTopicAreas = new List<TopicArea>() { childrenDto };
                    }
                    else
                    {
                        parentDto.ChildrenTopicAreas.Add(childrenDto);
                    }
                }
                if (!dict.ContainsKey(parent.Id))
                {
                    var mappedParent = MapParent(parent, children);
                    dict.Add(mappedParent.Id, mappedParent);
                }
            }
            else
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
        }
        
        res.AddRange(dict.Values);

        return res;
    }

    private static Public.DTO.V1.TopicArea MapParent(BLL.DTO.V1.TopicArea parent, BLL.DTO.V1.TopicArea children)
    {
        var childrenDto = new Public.DTO.V1.TopicArea()
        {
            Id = children.Id,
            Name = children.GetName()
        };
        var parentDto = new Public.DTO.V1.TopicArea()
        {
            Id = parent.Id,
            Name = parent.GetName(),
            ChildrenTopicAreas = new List<TopicArea>()
            {
                childrenDto
            }
        };
        return parentDto;
    }
    
}