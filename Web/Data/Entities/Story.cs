using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class Story : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public StoryFormat Format { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; }

    public Guid? PreviewId { get; set; }
    public File? Preview { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<StoryScore> StoryScores { get; set; } = new List<StoryScore>();
    public ICollection<StoryView> StoryViews { get; set; } = new List<StoryView>();
    public ICollection<StoryActivity> StoryActivities { get; set; } = new List<StoryActivity>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum StoryFormat
{
    Text = 1,
    Video = 2,
}