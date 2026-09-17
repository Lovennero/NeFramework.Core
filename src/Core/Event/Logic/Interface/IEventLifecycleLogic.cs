namespace Framework.Core.Event
{
    public interface IEventLifecycleLogic
    {
        EventRecord Subscribe<T>(string eventKey, EventHandlerAsync<T> handler) where T : IMsg;
        void Unsubscribe<T>(string eventKey, EventHandlerAsync<T> handler) where T : IMsg;
        void Unsubscribe<T>(string eventKey, int id) where T : IMsg;
    }
}