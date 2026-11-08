using System.Diagnostics;
using FastEndpoints;
using Infrastructure;

namespace Api.Endpoints;

public record HealthApplicationReport(bool IsDatabaseWorking, bool IsSystemWorking, long responseTime);

public class Health(ApplicationDbContext dbContext) : EndpointWithoutRequest
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var stopWatch = Stopwatch.StartNew();
        var canConnect = await dbContext.Database.CanConnectAsync(ct);
        stopWatch.Stop();
       await Send.OkAsync(new HealthApplicationReport(canConnect, true, stopWatch.ElapsedMilliseconds), ct);
    }

    public override void Configure()
    {
        Get("/api/health");
        AllowAnonymous();
    }
}