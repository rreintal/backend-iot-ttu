namespace Public.DTO.V1.Mappers;

public class ReturnTopicAreaMapper
{
    public static Public.DTO.V1.TopicArea Map(BLL.DTO.V1.TopicArea entity)
    {
        // TODO - kui kihte on rohkem kui 2!
        var res = new Public.DTO.V1.TopicArea()
        {
            Id = entity.Id,
            Name = entity.LanguageString!.LanguageStringTranslations.First().TranslationValue,
        };
        if (entity.ParentTopicAreaId != null)
        {
            res.ParentTopicId = entity.ParentTopicAreaId;
        }
        // TODO - parentil peaksid childid olema listina!
        /*
        if (entity.ParentTopicArea != null)
        {
            res.ParentTopicArea = Map(entity.ParentTopicArea);
        }
        */
        return res;
    }
}