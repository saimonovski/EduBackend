using Application.Dto;

using Application.Interfaces;
using Domain.Categories;
using Domain.Questions;
using FastEndpoints;

namespace Api.Endpoints.Questions;

public  record GeneratorRequest(int Count, int TopicId, QuestionType QuestionType,int Difficulty, int[] SubtopicsIds, bool Save = true);

public class GenerateQuestionsEndpoint(IQuestionService service) : Endpoint<GeneratorRequest, List<Domain.Questions.Question>>
{

    public override void Configure()
    {
        Post("api/questions/generate-questions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GeneratorRequest generatorRequest, CancellationToken ct)
    {
        Category category = null; // todo generatorRequest.TopicId
        Category[] categories = null; // todo generatorRequest.TopicId
        var result = await service.GenerateQuestionsAsync(generatorRequest.Count,category,generatorRequest.Difficulty, categories); //todo create service for categories
        if (!result.IsSuccess)
        {
            await Send.ErrorsAsync(400,ct);
            await Console.Error.WriteLineAsync(result.ErrorMessage);
            return;
        }

        var questions = result.Value!;
        
        
        await Send.OkAsync(questions, ct);
    }
}