namespace Framework.Core.Request
{
    public interface IRequestLifecycleLogic
    {
        RequestRecord AddRequest(string requestKey, string groupKey, int priority, RequestConfig config);
        void RemoveRequest(string requestKey);

    }
}