using Web.Data.Entities;

namespace Web.ViewModels.History;

public class HistoryNewRequest
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public Guid? PreviewId { get; set; }
    public HistoryFormat Format { get; set; }
    public ICollection<Guid> ActivityIds { get; set; }
}