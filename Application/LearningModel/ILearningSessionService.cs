using System.Text.Json.Serialization;
using Domain.LearningModel;
using Domain.Users;

namespace Application.LearningModel;

public record AnswerCheckerResponse(
    [property: JsonPropertyName("correct")] bool IsCorrect,
    [property: JsonPropertyName("solution_steps")] List<string> SolutionSteps,
    [property: JsonPropertyName("score")] double Score,
    [property: JsonPropertyName("explanation")] string Explanation
);
public interface ILearningSessionService
{
    public Task<(int StatusCode, LearningSession? learningSession)> CreateLearningSession(User user, string category,
        CancellationToken ct);
    Task<bool> EndLearningSession(string userId, CancellationToken ct);
    Task<LearningSession?> GetLearningSession(string userId, CancellationToken ct);
    Task<(AnswerCheckerResponse?,int StatusCode)> CheckAnswer(long responseTime, string taskId, string userId, string answer,
        CancellationToken ct);
    
}