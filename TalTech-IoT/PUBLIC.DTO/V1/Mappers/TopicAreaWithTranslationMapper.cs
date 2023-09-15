using App.Domain;

namespace Public.DTO.V1.Mappers;

public class TopicAreaWithTranslationMapper
{
    
    public static List<Public.DTO.V1.TopicAreaWithTranslation> Map(List<BLL.DTO.V1.TopicArea> bllTopicAreas)
    {
        var dict = new Dictionary<Guid, Public.DTO.V1.TopicAreaWithTranslation>();
        var res = new List<Public.DTO.V1.TopicAreaWithTranslation>();
        foreach (var children in bllTopicAreas)
        {
            if (children.ParentTopicArea != null)
            {
                var parent = children.ParentTopicArea;
                if (dict.ContainsKey(parent.Id))
                {

                    var childrenDto = new Public.DTO.V1.TopicAreaWithTranslation()
                    {
                        Id = children.Id,
                        Content = new List<ContentDto>()
                        {
                            new ContentDto()
                            {
                                Value = children.GetName(LanguageCulture.ENG),
                                Culture = LanguageCulture.ENG
                            },
                            new ContentDto()
                            {
                                Value = children.GetName(LanguageCulture.EST),
                                Culture = children.GetCulture(LanguageCulture.EST)
                            }
                        }
                    };
                    var parentDto = dict[parent.Id];
                    if (parentDto.ChildrenTopicAreas == null)
                    {
                        parentDto.ChildrenTopicAreas = new List<TopicAreaWithTranslation>() { childrenDto };
                    }
                    else
                    {
                        parentDto.ChildrenTopicAreas.Add(childrenDto);
                    }
                    
                    
                    //parentDto.ChildrenTopicAreas = new List<TopicAreaWithTranslation>() { childrenDto };
                    //parentDto.ChildrenTopicAreas.Add(childrenDto);
                    
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
                    var parent = new TopicAreaWithTranslation()
                    {
                        Id = children.Id,
                        Content = new List<ContentDto>()
                        {
                            new ContentDto()
                            {
                                Value = children.GetName(LanguageCulture.ENG),
                                Culture = LanguageCulture.ENG
                            },
                            new ContentDto()
                            {
                                Value = children.GetName(LanguageCulture.EST),
                                Culture = children.GetCulture(LanguageCulture.EST)
                            }
                        }
                    };
                    dict.Add(parent.Id, parent);
                }

            }
        }
        
        res.AddRange(dict.Values);

        return res;
    }

    private static Public.DTO.V1.TopicAreaWithTranslation MapParent(BLL.DTO.V1.TopicArea parent, BLL.DTO.V1.TopicArea children)
    {
        var childrenDto = new Public.DTO.V1.TopicAreaWithTranslation()
        {
            Id = children.Id,
            Content = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Value = children.GetName(LanguageCulture.ENG),
                    Culture = LanguageCulture.ENG
                },
                new ContentDto()
                {
                    Value = children.GetName(LanguageCulture.EST),
                    Culture = children.GetCulture(LanguageCulture.EST)
                }
            }
        };
        var parentDto = new Public.DTO.V1.TopicAreaWithTranslation()
        {
            Id = parent.Id,
            Content = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Value = parent.GetName(LanguageCulture.ENG),
                    Culture = LanguageCulture.ENG
                },
                new ContentDto()
                {
                    Value = parent.GetName(LanguageCulture.EST),
                    Culture = parent.GetCulture(LanguageCulture.EST)
                }
            },
            ChildrenTopicAreas = new List<TopicAreaWithTranslation>()
            {
                childrenDto
            }
        };
        return parentDto;
    }
}