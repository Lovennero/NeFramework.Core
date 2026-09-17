namespace Framework.Core.ObjectPool
{
    public interface IPoolLifecycleLogic
    {
        public PoolRecord CreatePool<T>(string poolName, IObjectProvider<T> provider, PoolParams poolParams) where T : class, IObject;
        public void ReleasePool(string poolName);
    }
}