using Application.Entity;
using Application.Interfaces;
using FastEndpoints;

namespace Api.Endpoints.Questions;


public  record CheckerRequest(Dictionary<int, string[]> QuestionsAnswers, int UserId);

public class CheckQuestionAnswersEndpoint(IQuestionService service)
    : Endpoint<CheckerRequest, Result<Dictionary<int, bool>>>
{

    public override void Configure()
    {
        Post("api/questions/check-answers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CheckerRequest checkerRequest, CancellationToken ct)
    {
        
        
        var result = await service.CheckAnswersAsync(checkerRequest.QuestionsAnswers);
        
        if (!result.IsSuccess)
        {
            await Send.NotFoundAsync(ct);
            await Console.Error.WriteLineAsync(result.ErrorMessage);
            return;
        }
        
        var checkedAnswers = result.Value!;
        await Send.OkAsync(Result<Dictionary<int,bool>>.Success(checkedAnswers), ct);
    }
}