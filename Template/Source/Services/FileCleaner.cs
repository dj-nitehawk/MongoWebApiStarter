namespace MongoWebApiStarter.Services;

public class FileCleanerService : BackgroundService
{
    //todo: remove this background service and let mongodb auto purge unlinked images with a TTL index

    readonly ILogger _log;

    public FileCleanerService(ILogger<FileCleanerService> log)
    {
        _log = log;
        log.LogWarning("{msg}", "FILE CLEANER SERVICE HAS STARTED..." + Environment.NewLine);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(6), cancellation);

            var imageCount = await Logic.Image.DeleteUnlinkedAsync();

            if (imageCount > 0)
            {
                _log.LogWarning(
                    "CLEANED: {imageCount} Images at {time} on {date}",
                    imageCount,
                    DateTime.UtcNow.ToTimePart(),
                    DateTime.UtcNow.ToDatePart());
            }
        }
    }
}