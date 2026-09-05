using Application.Entity;
using Application.Interfaces;
using Domain.Questions;

namespace EduBackend_Test;

/*
 * Brak pytania wyrzuci wyjątek (KeyNotFoundException):
W GetByIdAsync i GetAllById odwołujesz się bezpośrednio przez _questions[id]. Jeśli w testach poprosisz o ID, którego nie ma w słowniku, test wywali się z błędem braku klucza zamiast zwrócić null lub pustą listę.

Poprawka: Użyj _questions.TryGetValue(id, out var question) w GetByIdAsync, a w GetAllById dodawaj tylko istniejące obiekty lub użyj LINQ.

In-memory state pobierany przez referencję:
GetAllAsync() zwraca bezpośrednio _questions.Values. Modyfikacja obiektu pobranego z listy zmieni go bezpośrednio w słowniku (co w testach jest ok, ale warto o tym pamiętać).
 */

public class MockQuestionRepository : IQuestionRepository
{
     private readonly Dictionary<int, Question> _questions = new();
    
    public  Task<Result<Question>> UpdateAsync(Question question)
    {
        _questions[question.Id] = question;
        return Task.FromResult(Result<Question>.Success(question));
    }

    public Task RemoveAsync(Question question)
    {
        _questions.Remove(question.Id);
        return Task.FromResult(question);
    }

    public Task<Result<Question>> RemoveAsync(int id)
    {
        var removed = _questions[id];
        _questions.Remove(id);
        return Task.FromResult(Result<Question>.Success(removed));
    }

    public Task<Result<Question>> GetById(int id)
    {
        return Task.FromResult( Result<Question>.Success(_questions[id]));
    }

    public Task<Result<IEnumerable<Question>>> GetAll()
    {
        return Task.FromResult(Result<IEnumerable<Question>>.Success(_questions.Values));
    }

    public Task<Result<IEnumerable<Question>>> GetAllByIds(int[] ids)
    {
        List<Question> questions = [];
        questions.AddRange(ids.Select(key => _questions[key]));

        return Task.FromResult(Result<IEnumerable<Question>>.Success(questions));
    }
}