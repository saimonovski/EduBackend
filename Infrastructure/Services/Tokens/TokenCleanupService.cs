using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Tokens;

public sealed partial class TokenCleanupService(IServiceScopeFactory scopeFactory, ILogger<TokenCleanupService> logger) : BackgroundService
{
   

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
        
        
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanUpExpiredTokens(stoppingToken);
                await Task.Delay(600000 , stoppingToken);
            }
        }
        private async Task CleanUpExpiredTokens(CancellationToken stoppingToken)
        {
           await  using var scope = scopeFactory.CreateAsyncScope();
           var dbContext =  scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
           var deletedTokens =  await dbContext.RefreshTokens.Where(x => x.ExpiresAt < DateTimeOffset.UtcNow).ExecuteDeleteAsync(stoppingToken);
           LogTimeRemovedIntRefreshTokens(DateTimeOffset.Now, deletedTokens);
        }

        [LoggerMessage(LogLevel.Information, "[{time}] Removed: {int}, refresh tokens.")]
        partial void LogTimeRemovedIntRefreshTokens(DateTimeOffset time, int @int);
}
