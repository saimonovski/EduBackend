
using Application.Entity;
using Application.Questions;
using Domain.Categories;
using Domain.Questions;

namespace Infrastructure.Services;

public class QuestionService(IQuestionRepository questionRepository) : IQuestionService
{
    public async Task<Result<bool>> CheckAnswersAsync(int questionId, params string[] answers)
    {
        var question = await questionRepository.GetByIdAsync(questionId);

        if (question is null)
        {
            return Result<bool>.Failure(["Question not found"]);
        }

        return Result<bool>.Success(question.CheckAnswers(answers));
    }

    public async Task<Result<Dictionary<int, bool>>> CheckAnswersAsync(
        Dictionary<int, string[]> questionsAnswers)
    {
        var ids = questionsAnswers.Keys.ToArray();
        var questions = (await questionRepository.GetAllByIdsAsync(ids)).ToList();

        if (questions.Count != ids.Length)
        {
            return Result<Dictionary<int, bool>>.Failure(
                ["One or more questions were not found"]);
        }

        var checkedAnswers = new Dictionary<int, bool>();

        foreach (var question in questions)
        {
            checkedAnswers.Add(
                question.Id,
                question.CheckAnswers(questionsAnswers[question.Id])
            );
        }

        return Result<Dictionary<int, bool>>.Success(checkedAnswers);
    }

    public Task<Result<List<Question>>> GenerateQuestionsAsync(
        int number,
        Category category,
        int difficulty,
        Category[] subcategories)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Question>> GetQuestionAsync(int questionId)
    {
        var question = await questionRepository.GetByIdAsync(questionId);

        if (question is null)
        {
            return Result<Question>.Failure(["Question not found"]);
        }

        return Result<Question>.Success(question);
    }

    public async Task<Result<IEnumerable<Question>>> GetQuestionsAsync(int[] questionIds)
    {
        var questions = (await questionRepository.GetAllByIdsAsync(questionIds)).ToList();

        if (questions.Count != questionIds.Length)
        {
            return Result<IEnumerable<Question>>.Failure(
                ["One or more questions were not found"]);
        }

        return Result<IEnumerable<Question>>.Success(questions);
    }

    public async Task<Result<IEnumerable<Question>>> GetAllQuestionsAsync()
    {
        var questions = await questionRepository.GetAll();

        return Result<IEnumerable<Question>>.Success(questions);
    }
}
