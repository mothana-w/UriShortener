using UriShortener.Data.Model;

namespace UriShortener.Services.Background;

public class ExpiredUriCleanupService(ILogger<ExpiredUriCleanupService> _logger, IServiceProvider _service) : BackgroundService
{
  private readonly TimeSpan _interval = TimeSpan.FromHours(24);
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation($"STARTED: Execution of {nameof(ExpiredUriCleanupService)}.");

    while (!stoppingToken.IsCancellationRequested){
      try{
        using (var scope = _service.CreateScope()){
          var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
          var now = DateTime.UtcNow;
          var expiredUris = dbContext.ShortenedURIs.Where(shUri => shUri.ValidFor <= now);
          dbContext.RemoveRange(expiredUris);
          await dbContext.SaveChangesAsync();

          _logger.LogInformation($"SUCCEEDED: Execution of {nameof(ExpiredUriCleanupService)}.");
        }
      }catch{
        _logger.LogInformation($"FAILD: Execution of {nameof(ExpiredUriCleanupService)}.");
      }

      await Task.Delay(_interval, stoppingToken);
    }
  }
}