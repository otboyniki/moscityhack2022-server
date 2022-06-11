namespace Web.ViewModels.Story;

public class StoryDetailResponse
{
    public string CompanyName { get; set; }
    public DateTime Date { get; set; }

    public int Score { get; set; }
    public bool? IsPositiveScore { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public ICollection<StoryCommentItem> Comments { get; set; }
}

public class StoryCommentItem
{
    public string FullName { get; set; }
    public DateTime Date { get; set; }
    public string Text { get; set; }

    public int Score { get; set; }
    public bool? IsPositiveScore { get; set; }

    public string? AvatarPath { get; set; }
}