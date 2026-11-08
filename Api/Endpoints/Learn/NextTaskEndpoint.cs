using System.Security.Claims;
using Application.LearningModel;
using FastEndpoints;

namespace Api.Endpoints.Learn;

public class NextTaskEndpoint(ILearningSessionService learningSessionService)
    : EndpointWithoutRequest<StartSessionResponse>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var learningSession = await learningSessionService.GetLearningSession(userId, ct);
        if (learningSession == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        if (learningSession.Tasks.Count == 0)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        await Send.OkAsync(new StartSessionResponse(learningSession.Tasks[0]), ct);
    }

    public override void Configure()
    {
        Get("/api/learn/next-task");
        Roles("User");
    }
}
