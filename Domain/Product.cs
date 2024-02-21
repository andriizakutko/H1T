namespace Domain;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string SubName { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public float? OldPrice { get; set; }
    public float Price { get; set; }
    public int? Discount { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public string Gender { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<Image> Images { get; set; }
}