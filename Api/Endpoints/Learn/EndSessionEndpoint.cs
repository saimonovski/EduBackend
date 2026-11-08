using System.Security.Claims;
using Application.LearningModel;
using FastEndpoints;

namespace Api.Endpoints.Learn;

public class EndSessionEndpoint(ILearningSessionService learningSessionService) : EndpointWithoutRequest
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var ended = await learningSessionService.EndLearningSession(userId, ct);
        if (!ended)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }

    public override void Configure()
    {
        Post("/api/learn/end-session");
        Roles("User");
    }
}
