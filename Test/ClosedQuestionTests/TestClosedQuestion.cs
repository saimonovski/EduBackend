using Domain.Questions;

namespace EduBackend_Test.ClosedQuestionTests;

public class TestClosedQuestion(int id) : ClosedQuestion(id)
{
    private static int _count ;
    
    public static TestClosedQuestion Create()
    {
        _count++;
        return new TestClosedQuestion(_count);
    }
}