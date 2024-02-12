namespace Public.DTO.V1.FeedPage;

public class PostFeedPageCategoryWithPageIdentifier
{
        public string FeedPageIdentifier { get; set; } = default!; 
        public List<ContentDto> Title { get; set; } = default!;
}