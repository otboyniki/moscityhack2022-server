namespace Web.Data.Entities;

public class StoryScore
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid StoryId { get; set; }
    public Story Story { get; set; }

    public bool Positive { get; set; }
}