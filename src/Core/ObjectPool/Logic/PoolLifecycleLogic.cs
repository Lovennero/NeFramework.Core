using System;

namespace Framework.Core.ObjectPool
{
    internal sealed class PoolLifecycleLogic : IPoolLifecycleLogic
    {
        private const string Tag =  "Pool Lifecycle Logic";
        
        private readonly ObjectPoolModel _model;
        
        public PoolLifecycleLogic(ObjectPoolModel model)
        {
            _model = model;
        }

        public PoolRecord CreatePool<T>(string poolName, IObjectProvider<T> provider, PoolParams poolParams) where T : class, IObject
        {
            var pools = _model.Pools;
            if (pools.TryGetValue(poolName, out var pool))
            {
                throw new InvalidOperationException($"[{Tag}]: Pool {poolName} already exists.");
            }
            
            var objectPool = new ObjectPool<T>(poolName, provider, poolParams);
            pool = new PoolRecord(poolName, poolParams.Strategy, objectPool);
            pools[poolName] = pool;
            return pool;
        }

        public void ReleasePool(string poolName)
        {
            var pools = _model.Pools;
            if (!pools.TryGetValue(poolName, out var pool))
            {
                throw new InvalidOperationException($"[{Tag}]: Pool {poolName} does not exist.");
            }
            
            pool.Pool.Release();
            pool.IsValid = false;
            pools.Remove(poolName);
        }
    }
}