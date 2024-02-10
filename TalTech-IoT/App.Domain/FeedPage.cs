using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Base.Domain;

namespace App.Domain;

public class FeedPage : DomainEntityId
{
    public string FeedPageName { get; set; } = default!;
    public ICollection<FeedPageCategory>? FeedPageCategories { get; set; }
}