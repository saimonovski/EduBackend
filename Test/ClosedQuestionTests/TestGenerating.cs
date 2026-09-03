using Application.Interfaces;
using Infrastructure.Services;

namespace EduBackend_Test.ClosedQuestionTests;

public class TestGenerating
{
    private  IQuestionRepository _questionRepository;
    private  IQuestionService _questionService;

    [SetUp]
    public void Setup()
    {
        _questionRepository = new MockQuestionRepository();
        _questionService = new QuestionService(_questionRepository);
    }
    
    
}