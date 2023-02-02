namespace MongoWebApiStarter.Services;

public class FileCleanerService : BackgroundService
{
    //todo: remove this background service and let mongodb auto purge unlinked images with a TTL index

    private readonly ILogger log;

    public FileCleanerService(ILogger<FileCleanerService> log)
    {
        this.log = log;
        log.LogWarning("FILE CLEANER SERVICE HAS STARTED..." + Environment.NewLine);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(6), cancellation);

            var imageCount = await Logic.Image.DeleteUnlinkedAsync();

            if (imageCount > 0)
                log.LogWarning($"CLEANED: {imageCount} Images at {DateTime.UtcNow.ToTimePart()} on {DateTime.UtcNow.ToDatePart()}" + Environment.NewLine);
        }
    }
}