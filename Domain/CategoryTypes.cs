namespace Domain;

public class CategoryTypes
{
    public string Category { get; set; }
    public ICollection<string> Types { get; set; }
}