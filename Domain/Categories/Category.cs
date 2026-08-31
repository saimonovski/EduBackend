namespace Domain.Categories;

public class Category
{
    private int Id {get;}
    private string Name{get; set;}
    private string Description{get; set;}
    private List<String> Topics { get;  } = new List<string>();
    private List<Category> SubCategories { get; } = new List<Category>();

    public Category(int Id)
    {
        this.Id = Id;
    }
    
}