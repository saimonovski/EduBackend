using Application.Interfaces;
using Domain.Categories;
using Domain.Questions;

namespace Infrastructure.Services;

public class QuestionService : IQuestionService
{
    IQuestionRepository _questionRepository;

    public QuestionService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<bool> CheckAnswers(int questionId , params string[] answers)
    {
        var task = _questionRepository.GetById(questionId);
        
        Question question = await task;
        
       return question.CheckAnswers(answers);
    }

    public Task<List<Question>> GenerateQuestions(int number, Category category, int difficulty, Category[] subcategories)
    {
        throw new NotImplementedException();
    }
}