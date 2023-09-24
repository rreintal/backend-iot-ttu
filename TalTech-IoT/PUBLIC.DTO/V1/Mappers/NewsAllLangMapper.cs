using App.Domain;
using Content = BLL.DTO.V1.Content;

namespace Public.DTO.V1.Mappers;

public class NewsAllLangMapper
{
    public static NewsAllLangs Map(BLL.DTO.V1.News entity)
    {
        return new NewsAllLangs()
        {
            Id = entity.Id,
            Author = entity.Author,
            Image = entity.Image,
            Body = LanguageCulture.ALL_LANGUAGES.Select(lang =>
                {
                    return new ContentDto()
                    {
                        Value = entity.GetContentValue(ContentTypes.BODY, lang),
                        Culture = lang
                    };
                }).ToList(),
            Title = LanguageCulture.ALL_LANGUAGES.Select(lang =>
            {
                return new ContentDto()
                {
                    Value = entity.GetContentValue(ContentTypes.TITLE, lang),
                    Culture = lang
                };
            }).ToList(),
            TopicAreas = GetTopicAreaMapper.Map(entity.TopicAreas)
        };
    }
}