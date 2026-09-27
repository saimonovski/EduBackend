using Application.Entity;
using Domain.Questions;

namespace Application.Interfaces;

public interface IQuestionRepository
{
    Task<Question?> UpdateAsync(Question question);
    Task RemoveAsync(Question question);
    Task<Question?> RemoveAsync(int id);
    Task<Question?> GetByIdAsync(int id);
    Task<IEnumerable<Question>?> GetAll();

    Task<IEnumerable<Question>?> GetAllByIdsAsync(int[] ids);
    
}