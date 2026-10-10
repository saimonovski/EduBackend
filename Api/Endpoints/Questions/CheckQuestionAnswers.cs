using System.Security.Claims;
using Application.Entity;
using Application.Questions;
using FastEndpoints;

namespace Api.Endpoints.Questions;


public  record CheckerRequest(Dictionary<int, string[]> QuestionsAnswers,  float? ResponseTime);

public class CheckQuestionAnswersEndpoint(IQuestionService service)
    : Endpoint<CheckerRequest, Result<Dictionary<int, bool>>>
{

    public override void Configure()
    {
        Post("api/questions/check-answers");
        Roles("User");
    }

    public override async Task HandleAsync(CheckerRequest checkerRequest, CancellationToken ct)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id == null)
        {
            await Send.ErrorsAsync(400, ct);
            return;
        }
        
        var result = await service.CheckAnswersAsync(checkerRequest.QuestionsAnswers);
        
        if (!result.IsSuccess)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        
        var checkedAnswers = result.Value!;
        await Send.OkAsync(Result<Dictionary<int,bool>>.Success(checkedAnswers), ct);
    }
}