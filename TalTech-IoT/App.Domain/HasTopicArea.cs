using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class HasTopicArea : DomainEntityId
{
    public Guid TopicAreaId { get; set; }
    public TopicArea? TopicArea { get; set; }
    
    public Guid? NewsId { get; set; }
    public News? News { get; set; }

}