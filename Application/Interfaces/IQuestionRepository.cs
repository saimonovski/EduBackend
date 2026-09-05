using Application.Entity;
using Domain.Questions;

namespace Application.Interfaces;

public interface IQuestionRepository
{
    Task<Result<Question>> UpdateAsync(Question question);
    Task RemoveAsync(Question question);
    Task<Result<Question>> RemoveAsync(int id);
    Task<Result<Question>> GetByIdAsync(int id);
    Task<Result<IEnumerable<Question>>> GetAll();

    Task<Result<IEnumerable<Question>>> GetAllByIdsAsync(int[] id);
    
}