using Application.Entity;
using Application.Interfaces;
using Domain.Categories;
using Domain.Questions;

namespace Infrastructure.Services;

public class QuestionService(IQuestionRepository questionRepository) : IQuestionService
{
    public async Task<Result<bool>> CheckAnswersAsync(int questionId , params string[] answers)
    {
        var questionResult = await questionRepository.GetById(questionId);

        if (!questionResult.IsSuccess)
        {
            return Result<bool>.Failure(questionResult.ErrorMessage);
        }
        
        var question = questionResult.Value!;
        
        return Result<bool>.Success(question.CheckAnswers(answers));
    }

    public Task<Result<List<Question>>> GenerateQuestionsAsync(int number, Category category, int difficulty, Category[] subcategories)
    {
        
        throw new NotImplementedException();
    }
    
} 