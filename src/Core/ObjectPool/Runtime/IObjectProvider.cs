namespace Framework.Core.ObjectPool
{
    public interface IObjectProvider<T> where T : class, IObject
    {
        T Create();
        void Destroy(T obj);
    }
}