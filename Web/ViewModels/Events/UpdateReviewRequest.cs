namespace Web.ViewModels.Events;

public class UpdateReviewRequest
{
    public string Text { get; set; } = null!;

    public int CompanyRate { get; set; }
    public int GoalComplianceRate { get; set; }
}