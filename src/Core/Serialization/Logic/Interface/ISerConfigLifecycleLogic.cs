namespace Framework.Core.Serialization
{
    public interface ISerConfigLifecycleLogic
    {
        void Register<T>(string configKey, ISerSerializer<T> serSerializer, params ISerProcessor[] processors);
        bool Unregister<T>(string configKey);
    }
}