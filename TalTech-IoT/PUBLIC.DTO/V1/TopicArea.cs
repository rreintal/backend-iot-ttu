using System.ComponentModel.DataAnnotations;
using Base.Domain;
namespace Public.DTO.V1;

public class TopicArea : DomainEntityId
{
    public string? Name { get; set; }
    public List<TopicArea>? ChildrenTopicAreas { get; set; }
    
}

