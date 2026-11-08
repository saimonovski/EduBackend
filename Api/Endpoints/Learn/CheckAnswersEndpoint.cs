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

public record CheckAnswersRequest(long responseTime,string presentationId, string answerId );


public class CheckAnswersEndpoint(IUserService userService, ILearningSessionService learningSessionService) : Endpoint<CheckAnswersRequest, AnswerCheckerResponse>
{
    
    public override async Task HandleAsync(CheckAnswersRequest req, CancellationToken ct)
    {
       
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        
        var (response, statusCode) = await learningSessionService.CheckAnswer(req.responseTime,req.presentationId, id, req.answerId ,ct);
        if (statusCode != 200 ||  response == null)
        {
            await Send.ErrorsAsync(statusCode, ct);
            return;
        }
        await Send.OkAsync(response, ct);
    }

    public override void Configure()
    {
        Post("/api/learn/check-answers");
        Roles("User");
    }
}