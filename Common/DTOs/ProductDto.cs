namespace Common.DTOs;

public class ProductDto
{
    public string Name { get; set; }
    public string SubName { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public float? OldPrice { get; set; }
    public float Price { get; set; }
    public int? Discount { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public string Gender { get; set; }
    public string[] ImageUrls { get; set; }
}