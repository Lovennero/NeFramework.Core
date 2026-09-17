using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.ObjectPool
{
    internal sealed class PoolLogic : IPoolLogic
    {
        private const string Tag = "Pool Logic";

        private readonly ObjectPoolModel _model;
        
        public PoolLogic(ObjectPoolModel model)
        {
            _model = model;
        }

        public ValueTask<T> SpawnAsync<T>(string poolName, CancellationToken ct = default) where T : class, IObject
        {
            var pools =  _model.Pools;
            if (!pools.TryGetValue(poolName, out var poolRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Pool {poolName} does not exist.");
            }
            
            if (poolRecord.Pool is not ObjectPool<T> objectPool)
            {
                throw new InvalidOperationException($"[{Tag}]: Pool is not correct type.");
            }

            return objectPool.SpawnAsync(ct);
        }

        public void UnSpawn<T>(string poolName, T obj) where T : class, IObject
        {
            var pools =  _model.Pools;
            if (!pools.TryGetValue(poolName, out var poolRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Pool {poolName} does not exist.");
            }
            
            if (poolRecord.Pool is not ObjectPool<T> objectPool)
            {
                throw new InvalidOperationException($"[{Tag}]:Pool is not correct type.");
            }
            
            objectPool.UnSpawn(obj);
        }
    }
}