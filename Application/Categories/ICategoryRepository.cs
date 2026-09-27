using Application.Entity;
using Domain.Categories;

namespace Application.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task<Category?> DeleteAsync(int id);
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>?> GetAllAsync();
}