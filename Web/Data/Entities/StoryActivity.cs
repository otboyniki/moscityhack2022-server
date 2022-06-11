namespace Web.Data.Entities;

public class StoryActivity
{
    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; }

    public Guid StoryId { get; set; }
    public Story Story { get; set; }
}