using Application.Dto;
using Domain.Categories;
using Domain.Questions;

namespace Application.Interfaces;

public interface IQuestionService
{
    Task<bool> CheckAnswers(int questionId,params string[] answer);
    Task<List<Question>> GenerateQuestions(int number, Category category, int difficulty, Category[] subcategories);
    
}