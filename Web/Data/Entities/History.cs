using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class History : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public HistoryFormat Format { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; }

    public Guid? PreviewId { get; set; }
    public File? Preview { get; set; }

    public ICollection<HistoryComment> HistoryComments { get; set; } = new List<HistoryComment>();
    public ICollection<HistoryScore> HistoryScores { get; set; } = new List<HistoryScore>();
    public ICollection<HistoryView> HistoryViews { get; set; } = new List<HistoryView>();
    public ICollection<HistoryActivity> HistoryInterests { get; set; } = new List<HistoryActivity>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum HistoryFormat
{
    Text = 1,
    Video = 2,
}