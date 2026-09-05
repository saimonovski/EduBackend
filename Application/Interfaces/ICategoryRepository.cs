using Application.Entity;
using Domain.Categories;

namespace Application.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task<Result<Category>> DeleteAsync(int id);
    Result<Category> GetByIdAsync(int id);
    Task<IEnumerable<Result<Category>>> GetAllAsync();
}