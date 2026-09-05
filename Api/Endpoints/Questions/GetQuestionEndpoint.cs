using Application.Interfaces;
using FastEndpoints;

namespace Api.Endpoints.Questions;


public  record QuestionRequest(int Id);

public class GetQuestionEndpoint(IQuestionService service) : Endpoint<QuestionRequest, Domain.Questions.Question>
{

    public override void Configure()
    {
        Get("api/questions/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QuestionRequest questionRequest, CancellationToken ct)
    {
        var result = await service.GetQuestionAsync(questionRequest.Id);
        if (!result.IsSuccess)
        {
            await Send.NotFoundAsync(ct);
            await Console.Error.WriteLineAsync(result.ErrorMessage);
            return;
        }

        var question = result.Value!;
        await Send.OkAsync(question, ct);
    }
}