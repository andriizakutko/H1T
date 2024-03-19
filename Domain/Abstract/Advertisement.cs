namespace Domain.Abstract;

public abstract class Advertisement : BaseEntity
{
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ModeratorOverviewStatus { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}