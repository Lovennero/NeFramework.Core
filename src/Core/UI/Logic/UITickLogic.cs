using Framework.Core.FrameTime;

namespace Framework.Core.UI
{
    internal sealed class UITickLogic:IUITickLogic
    {
        // === 内部参数 ===
        private float _autoReleaseTimer;

        // === 存储数据 ===
        private readonly UITickModel _tickModel;

        // === 原子操作 ===
        private readonly UIPoolReleaseOperator _releaseOp;
        
        public UITickLogic(UITickModel tickModel, UIPoolReleaseOperator releaseOp)
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
            TickUIRegular(logicTime, realTime);
            TickUIPoolRegular(realTime);
        }

        private void TickUIRegular(float logicTime, float realTime)
        {
            for (var i = 0; i < _tickModel.GroupCount; i++)
            {
                var uis = _tickModel.GroupUIs[i];
                var count = _tickModel.GroupUICounts[i];

                for (var j = 0; j < count; j++)
                {
                    var record = uis[j];
                    if (!record.IsValid) continue;
                    record.UI.OnUpdate(logicTime, realTime);
                }
            }
        }

        private void TickUIPoolRegular(float realTime)
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