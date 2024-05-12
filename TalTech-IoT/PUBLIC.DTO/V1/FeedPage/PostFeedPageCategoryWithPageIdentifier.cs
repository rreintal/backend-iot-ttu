using System.ComponentModel.DataAnnotations;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1.FeedPage;

public class PostFeedPageCategoryWithPageIdentifier
{ 
        [MinLength(2)]
        [MaxLength(64)]
        public string FeedPageIdentifier { get; set; } = default!; 
        [IncludesAllCultures]
        public List<ContentDto> Title { get; set; } = default!;
}