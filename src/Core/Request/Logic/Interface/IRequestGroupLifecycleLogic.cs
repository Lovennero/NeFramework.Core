namespace Framework.Core.Request
{
    public interface IRequestGroupLifecycleLogic
    {
        void CreateRequestGroup(string groupKey, int priority, RequestGroupConfig config);
        void RemoveRequestGroup(string groupKey);
    }
}