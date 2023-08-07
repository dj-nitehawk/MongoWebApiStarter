namespace Dom;

internal sealed class JobRecord : Entity, IJobStorageRecord
{
    public string QueueID { get; set; }
    public object Command { get; set; }
    public DateTime ExecuteAfter { get; set; }
    public DateTime ExpireOn { get; set; }
    public bool IsComplete { get; set; }
    public int FailureCount { get; set; }
}
