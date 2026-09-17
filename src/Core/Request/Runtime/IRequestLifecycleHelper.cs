namespace Framework.Core.Request
{
    public interface IRequestLifecycleHelper
    {
        IRequest AddRequest(string requestKey, RequestGroupConfig groupConfig, RequestConfig config);
        void RemoveRequest(string requestKey);
    }
}