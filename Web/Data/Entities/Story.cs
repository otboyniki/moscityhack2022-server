using System.ComponentModel.DataAnnotations.Schema;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

public abstract class Story : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public StoryFormat Format { get; set; }

    public Guid? PreviewId { get; set; }
    public File? Preview { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<StoryScore> StoryScores { get; set; } = new List<StoryScore>();
    public ICollection<StoryView> StoryViews { get; set; } = new List<StoryView>();
    public ICollection<StoryActivity> StoryActivities { get; set; } = new List<StoryActivity>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public abstract string FullName { get; }
}

public class CompanyStory : Story
{
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }

    public override string FullName => Company.Title;
}

public class UserStory : Story
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public override string FullName => User.FullName;
}

public enum StoryFormat
{
    Text = 1,
    Video = 2,
}