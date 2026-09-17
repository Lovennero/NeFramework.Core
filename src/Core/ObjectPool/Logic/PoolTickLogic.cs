namespace Framework.Core.ObjectPool
{
    public class PoolTickLogic:IPoolTickLogic
    {
        private const string Tag = "Pool Tick Logic";

        private readonly ObjectPoolTickModel  _tickModel;
        
        public PoolTickLogic(ObjectPoolTickModel tickModel)
        {
            _tickModel = tickModel;
        }
        
        public void Tick(float logicTime, float realTime)
        {
            TickPoolRegular(logicTime, realTime);
        }

        private void TickPoolRegular(float logicTime, float realTime)
        {
            for (var i = 0; i < _tickModel.PoolCount; i++)
            {
                var pool = _tickModel.Pools[i];
                pool.Pool.OnUpdate(logicTime, realTime);
            }
        }
    }
}