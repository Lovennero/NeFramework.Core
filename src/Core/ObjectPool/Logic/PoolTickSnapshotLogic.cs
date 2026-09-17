namespace Framework.Core.ObjectPool
{
    internal sealed class PoolTickSnapshotLogic : IPoolTickSnapshotLogic
    {
        private const string Tag =  "Pool Tick Snapshot Logic";

        private readonly ObjectPoolModel _model;
        private readonly ObjectPoolTickModel _tickModel;
        
        public PoolTickSnapshotLogic(ObjectPoolModel model,ObjectPoolTickModel tickModel)
        {
            _model = model;
            _tickModel = tickModel;
        }

        public void Capture()
        {
            var poolCount = _model.Pools.Count;
            if (_tickModel.Pools.Length < poolCount)
            {
                _tickModel.Pools = new PoolRecord[poolCount];
            }
            _model.Pools.Values.CopyTo(_tickModel.Pools, 0);
            _tickModel.PoolCount = poolCount;
        }
    }
}