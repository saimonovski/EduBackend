using Domain.Questions;

namespace EduBackend_Test.ClosedQuestionTests;

public class TestClosedQuestion() : ClosedQuestion()
{
    private static int _count = 0;
    
    public static TestClosedQuestion Create()
    {
        _count++;
        return new TestClosedQuestion{Id = _count};
    }
}