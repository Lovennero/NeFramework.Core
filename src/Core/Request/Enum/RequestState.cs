namespace Framework.Core.Request
{
    public enum RequestState
    {
        Waiting,
        Running,
        Retry,
        Completed,
        Cancelled,
        Invalid
    }
}