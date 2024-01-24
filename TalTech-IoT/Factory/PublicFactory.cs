using App.Domain;
using Microsoft.IdentityModel.Tokens;
using Public.DTO.V1;

namespace Factory;

public static class PublicFactory
{
    
    // MARK: CREATE - PostNewsDto 
    
    public static PostNewsDto CreatePostNews()
    {
        return new PostNewsDto();
    }
    
    public static PostNewsDto SetContent(this PostNewsDto dto, string contentType, Dictionary<string, string> contentMap)
    {
        List<ContentDto> contentDtos = new List<ContentDto>();

        foreach (var (translationValue, languageCulture) in contentMap)
        {
            ContentDto content = new ContentDto()
            {
                Value = translationValue,
                Culture = languageCulture
            };
            contentDtos.Add(content);
        }

        switch (contentType)
        {
            case ContentTypes.BODY:
                dto.Body = contentDtos;
                break;
            case ContentTypes.TITLE:
                dto.Title = contentDtos;
                break;
        }

        return dto;
    }
    
    public static PostNewsDto SetContent(
        this PostNewsDto dto, 
        string type,
        string culture,
        string contentString)
    {
        ContentDto content = new ContentDto()
        {
            Value = contentString,
            Culture = culture
        };
        
        // TODO: refactor!
        switch (type)
        {
            case ContentTypes.BODY:
                int bodyIndex = dto.Body.FindIndex(c => c.Culture == culture);

                if (bodyIndex != -1)
                {
                    dto.Body[bodyIndex] = content;
                }
                else
                {
                    dto.Body.Add(content);
                }
                break;
            case ContentTypes.TITLE:
                int titleIndex = dto.Body.FindIndex(c => c.Culture == culture);

                if (titleIndex != -1)
                {
                    dto.Body[titleIndex] = content;
                }
                else
                {
                    dto.Body.Add(content);
                }
                break;
        }

        return dto;
    }

    public static PostNewsDto SetAuthor(this PostNewsDto dto, string author)
    {
        dto.Author = author;
        return dto;
    }


    public static PostNewsDto SetImage(this PostNewsDto dto, string image)
    {
        dto.Image = image;
        return dto;
    }

    public static PostNewsDto SetTopicArea(this PostNewsDto dto, Public.DTO.V1.TopicArea topicArea)
    {
        if (dto.TopicAreas == null)
        {
            dto.TopicAreas = new List<Public.DTO.V1.TopicArea>() { topicArea };
        }
        else
        {
            dto.TopicAreas.Add(topicArea);
        }

        return dto;
    }

    public static Public.DTO.V1.TopicArea TopicArea(Guid id)
    {
        return new Public.DTO.V1.TopicArea()
        {
            Id = id
        };
    }

    public static PostNewsDto SetTopicAreaId(this PostNewsDto dto, string id)
    {
        var topicArea = new Public.DTO.V1.TopicArea()
        {
            Id = Guid.Parse(id)
        };
        
        if (dto.TopicAreas == null)
        {
            dto.TopicAreas = new List<Public.DTO.V1.TopicArea>()
            {
                topicArea
            };
        }
        else
        {
            dto.TopicAreas.Add(topicArea);
        }

        return dto;
    }

}