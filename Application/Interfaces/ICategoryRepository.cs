using Domain.Categories;

namespace Application.Interfaces;

public interface ICategoryRepository
{
    Task<Category> Add(Category category);
    Task<Category> Update(Category category);
    Task<Category> Delete(int id);
    Task<Category> GetById(int id);
    Task<IEnumerable<Category>> GetAll();
}