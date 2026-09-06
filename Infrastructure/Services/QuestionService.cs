using System.Collections;
using Application.Entity;
using Application.Interfaces;
using Domain.Categories;
using Domain.Questions;

namespace Infrastructure.Services;

public class QuestionService(IQuestionRepository questionRepository) : IQuestionService
{
    public async Task<Result<bool>> CheckAnswersAsync(int questionId , params string[] answers)
    {
        var questionResult = await questionRepository.GetByIdAsync(questionId);

        if (!questionResult.IsSuccess)
        {
            return Result<bool>.Failure(questionResult.ErrorMessage);
        }
        
        var question = questionResult.Value!;
        
        return Result<bool>.Success(question.CheckAnswers(answers));
    }

    public async Task<Result<Dictionary<int,bool>>> CheckAnswersAsync(Dictionary<int, string[]> questionsAnswers)
    {
         var keys = questionsAnswers.Keys.ToArray();
         var databaseResult = await questionRepository.GetAllByIdsAsync(keys);

         if (!databaseResult.IsSuccess)
         {
             return Result<Dictionary<int, bool>>.Failure(databaseResult.ErrorMessage);
         }
         
         var checkedAnswers = new Dictionary<int, bool>();
         
         var questions = databaseResult.Value!.ToList();
         questions.ForEach((question =>
             checkedAnswers.Add(question.Id,question.CheckAnswers(questionsAnswers[question.Id]))
         ));
         return  Result<Dictionary<int,bool>>.Success(checkedAnswers);
    }

    public Task<Result<List<Question>>> GenerateQuestionsAsync(int number, Category category, int difficulty, Category[] subcategories)
    {
        
        throw new NotImplementedException();
    }

    public Task<Result<Question>> GetQuestionAsync(int questionId)
    {
        return questionRepository.GetByIdAsync(questionId);
    }

    public Task<Result<IEnumerable<Question>>> GetQuestionsAsync(int[] questionId)
    {
        return questionRepository.GetAllByIdsAsync(questionId);
    }

    public Task<Result<IEnumerable<Question>>> GetAllQuestionsAsync()
    {
        return questionRepository.GetAll();
    }
} 