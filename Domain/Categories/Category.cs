namespace Domain.Categories;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string Description{get; set;}
    public List<String> Topics { get;  } = new List<string>();
    public List<Category> SubCategories { get; } = new List<Category>();

  
    
}