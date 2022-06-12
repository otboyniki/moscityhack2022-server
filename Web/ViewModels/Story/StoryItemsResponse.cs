using System.Linq.Expressions;
using Web.Data.Entities;

namespace Web.ViewModels.Story;

public class StoryItemsResponse
{
    public ICollection<StoryDto> Items { get; set; }
}

public class StoryDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public int Score { get; set; }
    public int CommentsCount { get; set; }
    public int ViewsCount { get; set; }
    public Guid? PreviewId { get; set; }
    public StoryFormat Format { get; set; }
    public ICollection<StoryItemsActivityItem> Activities { get; set; }
    public DateTime Date { get; set; }

    public static Expression<Func<Data.Entities.Story, StoryDto>> Projection => story => new StoryDto
    {
        Id = story.Id,
        FullName = story.FullName,
        Title = story.Title,
        ShortDescription = story.ShortDescription,
        Score = story.StoryScores.Sum(x => x.Positive ? 1 : -1),
        CommentsCount = story.Comments.Count,
        ViewsCount = story.StoryViews.Count,
        PreviewId = story.PreviewId,
        Format = story.Format,
        Activities = story.StoryActivities.Select(x => new StoryItemsActivityItem
        {
            Title = x.Activity.Title,
            IconId = x.Activity.Icon == null ? null : x.Activity.Icon.Id,
        }).ToArray(),
        Date = story.CreatedAt,
    };
}

public class StoryItemsActivityItem
{
    public string Title { get; set; }
    public Guid? IconId { get; set; }
}