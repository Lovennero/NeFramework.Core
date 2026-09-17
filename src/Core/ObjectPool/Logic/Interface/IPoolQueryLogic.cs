namespace Framework.Core.ObjectPool
{
    public interface IPoolQueryLogic
    {
        bool HasPool(string poolName);
        PoolRecord GetPool(string poolName);
        bool TryGetPool(string poolName, out PoolRecord pool);
    }
}