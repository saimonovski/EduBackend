using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Api.Endpoints.Learn;
using Application.LearningModel;
using Domain.LearningModel;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class LearningSessionService(IConfiguration configuration, ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory) : ILearningSessionService
{
    private const string LearningModelApiUrl = "https://domatapp-learningmodel.onrender.com/api/v1";
    private const string EducationStage = "primary school";
    private const string CurriculumLevel = "STANDARD";
    private const string LearningPlansEndpoint = "/learning-plans";
    private const string TasksEndpoint = "/tasks";


    public async Task<(int StatusCode, LearningSession? learningSession)> CreateLearningSession(User user, string category, CancellationToken ct)
    {
        using var client = httpClientFactory.CreateClient();
        
        
        var createPlanEndpoint = LearningModelApiUrl + LearningPlansEndpoint;
        var grade = user.UserData.ClassLevel;
        var curriculumContext = new
            { grade = grade, education_stage = EducationStage, curriculum_level = CurriculumLevel };
        
        var learningPlanRequest = new { student_id = user.Id, selected_domain = category, curriculum_context = curriculumContext };
        
        HttpResponseMessage response = await client.PostAsJsonAsync(createPlanEndpoint, learningPlanRequest, ct);
        SuccessResponse? successData = null;
        ;
        if (response.IsSuccessStatusCode)
        {
            successData = await response.Content.ReadFromJsonAsync<SuccessResponse>(cancellationToken: ct);
            if (successData == null)
            {
                return (404, null);
            }
        }
        else 
        {
            
            ValidationErrorResponse? errorData = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>(cancellationToken: ct);
                
            if (errorData?.Detail != null)
            {
                Console.WriteLine("Błąd walidacji danych wejściowych:");
                foreach (var error in errorData.Detail)
                {
                    string location = string.Join(" -> ", error.Loc);
                    Console.WriteLine($" - Lokalizacja: {location}");
                    Console.WriteLine($"   Komunikat: {error.Msg}");
                    Console.WriteLine($"   Wartość wejściowa: {error.Input}");
                }
            }
            
            return (400, null);
        }

        var planId = successData!.PlanId;
        var learningSession = new LearningSession()
        {
            Category = category,
            Id = planId,
            UserId = user.Id,
            PresentationId = successData.PlanId
        };

        var tasksEndpoint = LearningModelApiUrl + LearningPlansEndpoint + "/" + planId + "/tasks?student_id=" + user.Id;
        var tasksResponse = await client.GetAsync(tasksEndpoint, ct);

        if (!tasksResponse.IsSuccessStatusCode)
        {
            return (404, null);
        }
        await using var responseStream = await tasksResponse.Content.ReadAsStreamAsync(ct);

        var tasks = new List<TaskParserUtil.SimplifiedTask>();
        await foreach (var task in TaskParserUtil.ParseTasksAsync(responseStream, ct))
        {
            tasks.Add(task);
        }

        learningSession.Tasks.AddRange(tasks);
        await dbContext.LearningSessions.AddAsync(learningSession, ct);
        await dbContext.SaveChangesAsync(ct);
        return (200, learningSession);
    }

    public async Task<bool> EndLearningSession(string userId, CancellationToken ct)
    {
        var learningSession = await dbContext.LearningSessions
            .FirstOrDefaultAsync(ls => ls.UserId == userId, ct);
        if (learningSession == null)
        {
            return false;
        }

        dbContext.LearningSessions.Remove(learningSession);
        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<LearningSession?> GetLearningSession(string userId, CancellationToken ct)
    {
        return await dbContext.LearningSessions.Where(ls => ls.UserId.Equals(userId)).Include(ls => ls.Tasks).FirstOrDefaultAsync(cancellationToken: ct);
    }
    
   
    public async Task<(AnswerCheckerResponse?, int StatusCode)> CheckAnswer(long responseTime, string presentationId, string userId, string answer, CancellationToken ct)
    {
        var learningSession = await GetLearningSession(userId, ct);
        if (learningSession == null) return (null, 404);


        var sessionId = learningSession.Id;
        var checkAnswersEndpoint =  LearningModelApiUrl  + TasksEndpoint+"/"+presentationId+"/answer";

        var payload = new
        {
            student_id = userId, session_id = sessionId, selected_option_id = answer,
            response_time_ms = responseTime, attempts = 1
        };

        using var client = httpClientFactory.CreateClient();

        var response = await client.PostAsJsonAsync(checkAnswersEndpoint, payload, ct);
        Console.WriteLine("=====================\n"+ await response.Content.ReadAsStringAsync(ct));
        if (!response.IsSuccessStatusCode)
        {
            return (null, 500);
        }

        var answerCheckerResponse = await response.Content.ReadFromJsonAsync<AnswerCheckerResponse>(cancellationToken: ct);
        learningSession.Tasks.RemoveAt(0);
        await dbContext.SaveChangesAsync(ct);
        return (answerCheckerResponse, 200);
    }
}

public record SuccessResponse(
    [property: JsonPropertyName("plan_id")] string PlanId
);

public record ValidationErrorResponse(
    [property: JsonPropertyName("detail")] List<ValidationErrorDetail> Detail
);

public record ValidationErrorDetail(
    [property: JsonPropertyName("loc")] List<object> Loc, 
    [property: JsonPropertyName("msg")] string Msg,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("input")] string Input,
    [property: JsonPropertyName("ctx")] Dictionary<string, object> Ctx 
);