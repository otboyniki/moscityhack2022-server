using Web.Data.Entities;

namespace Web.ViewModels.Story;

public class StoryNewRequest
{
    public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public Guid? PreviewId { get; set; }
    public StoryFormat Format { get; set; }
    public ICollection<Guid> ActivityIds { get; set; }
}