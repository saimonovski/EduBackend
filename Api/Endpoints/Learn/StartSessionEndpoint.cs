using System.Security.Claims;
using Application.Users;
using Domain.LearningModel;


using FastEndpoints;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.LearningModel;


namespace Api.Endpoints.Learn;

public record StartSessionRequest(string CategoryId);
public record StartSessionResponse(TaskParserUtil.SimplifiedTask Task);





public class StartSessionEndpoint(IUserService userService, ILearningSessionService learningSessionService) : Endpoint<StartSessionRequest, StartSessionResponse>
{
    
    public override async Task HandleAsync(StartSessionRequest req, CancellationToken ct)
    {
       
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var user = await userService.FindUserByIdAsync(id);
        

        if (user == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        await learningSessionService.EndLearningSession(id, ct);
        var category = req.CategoryId;
        var grade = user.UserData.ClassLevel;
        
        var (statusCode, learningSession) = await learningSessionService.CreateLearningSession(user, category, ct);
        if (statusCode != 200 ||  learningSession == null)
        {
            await Send.ErrorsAsync(statusCode, ct);
            return;
        }
        if (learningSession.Tasks.Count == 0)
        {
            await Send.NoContentAsync(ct);
            return;
        }
        await Send.OkAsync(new StartSessionResponse(learningSession.Tasks.First()), ct);
    }

    public override void Configure()
    {
        Post("/api/learn/start-session");
        Roles("User");
    }
}