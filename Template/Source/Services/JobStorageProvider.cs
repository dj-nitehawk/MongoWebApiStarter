using Dom;
using Order = MongoDB.Entities.Order;

namespace MongoWebApiStarter.Services;

internal sealed class JobStorageProvider : IJobStorageProvider<JobRecord>
{
    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> p)
    {
        return await DB
            .Find<JobRecord>()
            .Match(p.Match)
            .Sort(r => r.ID, Order.Ascending)
            .Limit(p.Limit)
            .ExecuteAsync(p.CancellationToken);
    }

    public Task MarkJobAsCompleteAsync(JobRecord r, CancellationToken ct)
    {
        return DB
            .Update<JobRecord>()
            .MatchID(r.ID)
            .Modify(r => r.IsComplete, true)
            .ExecuteAsync(ct);
    }

    public Task OnHandlerExecutionFailureAsync(JobRecord r, Exception exception, CancellationToken ct)
    {
        if (r.FailureCount > 100)
            return DB.DeleteAsync<JobRecord>(r.ID, cancellation: ct);

        var retryOn = DateTime.UtcNow.AddMinutes(1);
        var expireOn = retryOn.AddHours(4);

        return DB
            .Update<JobRecord>()
            .MatchID(r.ID)
            .Modify(b => b.Inc(r => r.FailureCount, 1)) //increment the failure count.
            .Modify(r => r.ExecuteAfter, retryOn) //slide the execute after to 1 min in future.
            .Modify(r => r.ExpireOn, expireOn) //slide the expire on to 4 hours from execute after time.
            .ExecuteAsync(ct);
    }

    public Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> p)
    {
        return DB.DeleteAsync(p.Match, cancellation: p.CancellationToken);
    }

    public Task StoreJobAsync(JobRecord r, CancellationToken ct)
    {
        return r.SaveAsync(cancellation: ct);
    }
}
