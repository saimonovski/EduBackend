using Application.Interfaces;
using FastEndpoints;

namespace Api.Endpoints.Questions;


public class GetAllQuestionsEndpoint(IQuestionService service) : EndpointWithoutRequest<List<Domain.Questions.Question>>
{

    public override void Configure()
    {
        Get("api/questions/all");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await service.GetAllQuestionsAsync();
        if (!result.IsSuccess)
        {
            await Send.NotFoundAsync(ct);
            await Console.Error.WriteLineAsync(result.ErrorMessage);
            return;
        }

        var questions = result.Value!;
        await Send.OkAsync(questions.ToList(), ct);
    }
}