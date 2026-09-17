using System;

namespace Framework.Core.ObjectPool
{
    internal class PoolQueryLogic : IPoolQueryLogic
    {
        private const string Tag = "Pool Query Logic";

        private readonly ObjectPoolModel _model;
        
        public PoolQueryLogic(ObjectPoolModel model)
        {
            _model = model;
        }

        public bool HasPool(string poolName)
        {
            var pools = _model.Pools;
            return pools.ContainsKey(poolName);
        }

        public PoolRecord GetPool(string poolName)
        {
            var pools = _model.Pools;
            if (!pools.TryGetValue(poolName, out var pool))
            {
                throw new InvalidOperationException($"[{Tag}]: Pool:{poolName} does not exist.");
            }
            return pool;
        }

        public bool TryGetPool(string poolName, out PoolRecord pool)
        {
            var pools = _model.Pools;
            return pools.TryGetValue(poolName, out pool);
        }
    }
}