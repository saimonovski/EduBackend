using Domain.Questions;
using FastEndpoints;

namespace Api.Controllers;


public record QuestionRequest(int Id);

public class GetQuestionEndpoint : Endpoint<QuestionRequest,Question>
{
    public override void Configure()
    {
        Get("api/questions/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QuestionRequest questionRequest, CancellationToken ct)
    {
        var question = new ClosedQuestion(questionRequest.Id);
        await Send.OkAsync(question, ct);
    }
}