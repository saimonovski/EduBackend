using Application.Entity;
using Application.Interfaces;
using Domain.Questions;
using EduBackend_Test.ClosedQuestionTests;

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
     private  readonly Dictionary<int, Question> _questions = new();

     public MockQuestionRepository()
     {
         var mainQuestion = TestClosedQuestion.Create();
         var mainQuestion2 = TestClosedQuestion.Create();
         var mainQuestion3 = TestClosedQuestion.Create();

         var subQuestion = TestClosedQuestion.Create();
         subQuestion.Answers = ["incorrect", "correct", "incorrect2"];
         subQuestion.CorrectAnswers.Add("correct");
         subQuestion.QuestionContext = "test";
         mainQuestion2.QuestionContext = "test";
         mainQuestion3.QuestionContext = "test";

         mainQuestion.Items.Add(subQuestion);
         mainQuestion.Answers = ["incorrect", "correct", "incorrect2", "incorrect3", "correct2"];
         mainQuestion.CorrectAnswers.Add("correct");
         mainQuestion.CorrectAnswers.Add("correct2");
        
          UpdateAsync(mainQuestion);
          UpdateAsync(mainQuestion2);
          UpdateAsync(mainQuestion3);
           UpdateAsync(subQuestion);
     }
    
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

    public Task<Result<Question>> GetByIdAsync(int id)
    {
        return Task.FromResult( Result<Question>.Success(_questions[id]));
    }

    public Task<Result<IEnumerable<Question>>> GetAll()
    {
        return Task.FromResult(Result<IEnumerable<Question>>.Success(_questions.Values));
    }

    public Task<Result<IEnumerable<Question>>> GetAllByIdsAsync(int[] ids)
    {
        List<Question> questions = [];
        questions.AddRange(ids.Select(key => _questions[key]));

        return Task.FromResult(Result<IEnumerable<Question>>.Success(questions));
    }
}