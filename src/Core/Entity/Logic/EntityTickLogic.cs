using Framework.Core.FrameTime;

namespace Framework.Core.Entity
{
    internal sealed class EntityTickLogic:IEntityTickLogic
    {
        private const string Tag = "Entity Tick Logic";
        
        // === 内部参数 ===
        private float _autoReleaseTimer;
        
        // === 存储数据 ===
        private readonly EntityTickModel _tickModel;
        
        // === 原子操作 ===
        private readonly EntityPoolReleaseOperator _releaseOp;

        public EntityTickLogic(EntityTickModel tickModel,EntityPoolReleaseOperator releaseOp)
        {
            // === 内部参数 ===
            _autoReleaseTimer = 2f;
            
            // === 存储数据 ===
            _tickModel = tickModel;
            
            // === 原子操作 ===
            _releaseOp = releaseOp;
        }
        
        public void Tick(float logicTime, float realTime)
        {
            TickEntityRegular(logicTime, realTime);
            TickEntityPoolRegular(realTime);
        }

        private void TickEntityRegular(float logicTime, float realTime)
        {
            for (var i = 0; i < _tickModel.GroupCount; i++)
            {
                var entities = _tickModel.GroupEntities[i];
                var count = _tickModel.GroupEntityCounts[i];

                for (var j = 0; j < count; j++)
                {
                    var record = entities[j];
                    if (record.LifePhase != ELifePhase.Active) continue;
                    if (record.ShowPhase != EShowPhase.Show) continue;
                    record.Entity.OnUpdate(logicTime, realTime);
                }
            }
        }

        private void TickEntityPoolRegular(float realTime)
        {
            _autoReleaseTimer -= realTime;
            if (_autoReleaseTimer > 0) return;
            _autoReleaseTimer = 2f;
            
            for (var i = 0; i < _tickModel.PoolCount; i++)
            {
                var record = _tickModel.Pools[i];
                if(!record.IsValid) continue;
                if(record.LastUseTime < 0) continue;
                if(FTime.GetRealRuntime() - record.LastUseTime < 30) continue;
                _releaseOp.Execute(record);
            }
        }
        
    }
}