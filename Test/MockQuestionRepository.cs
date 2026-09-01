using Application.Interfaces;
using Domain.Questions;

namespace EduBackend_Test;

/*
 * Brak pytania wyrzuci wyjątek (KeyNotFoundException):
W GetById i GetAllById odwołujesz się bezpośrednio przez _questions[id]. Jeśli w testach poprosisz o ID, którego nie ma w słowniku, test wywali się z błędem braku klucza zamiast zwrócić null lub pustą listę.

Poprawka: Użyj _questions.TryGetValue(id, out var question) w GetById, a w GetAllById dodawaj tylko istniejące obiekty lub użyj LINQ.

In-memory state pobierany przez referencję:
GetAll() zwraca bezpośrednio _questions.Values. Modyfikacja obiektu pobranego z listy zmieni go bezpośrednio w słowniku (co w testach jest ok, ale warto o tym pamiętać).
 */

public class MockQuestionRepository : IQuestionRepository
{
     private readonly Dictionary<int, Question> _questions = new();
    
    public Task<Question> Update(Question question)
    {
        _questions[question.Id] = question;
        return Task.FromResult(question);
    }

    public Task<Question> Remove(Question question)
    {
        _questions.Remove(question.Id);
        return Task.FromResult(question);
    }

    public Task<Question> GetById(int id)
    {
        return Task.FromResult( _questions[id]);
    }

    public Task<IEnumerable<Question>> GetAll()
    {
        return Task.FromResult<IEnumerable<Question>>(_questions.Values);
    }

    public Task<IEnumerable<Question>> GetAllById(int[] id)
    {
        List<Question> questions = [];
        foreach (int key in id)
        {
            questions.Add(_questions[key]);
        }

        return Task.FromResult<IEnumerable<Question>>(questions);
    }
}