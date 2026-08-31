using Domain.Questions;

namespace Application.Interfaces;

public interface IQuestionRepository
{
    Task<Question> Update(Question question);
    Task<Question> Remove(Question question);
    Task<Question> GetById(int id);
    Task<IEnumerable<Question>> GetAll();
    
}