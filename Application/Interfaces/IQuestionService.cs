using Application.Dto;
using Application.Entity;
using Domain.Categories;
using Domain.Questions;

namespace Application.Interfaces;

public interface IQuestionService
{
    Task<Result<bool>> CheckAnswersAsync(int questionId,params string[] answer);
    Task<Result<List<Question>>> GenerateQuestionsAsync(int number, Category category, int difficulty,
        Category[] subcategories);
    Task<Result<
    
    
}