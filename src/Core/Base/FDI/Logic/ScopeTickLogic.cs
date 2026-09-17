namespace Framework.Core.FrameDI
{
    internal sealed class ScopeTickLogic : IScopeTickLogic
    {
        private const string Tag = "Scope Cycle Logic";
        
        private readonly FDIModel _model;
        private readonly FDITickModel _tickModel;

        public ScopeTickLogic(
            FDIModel model,
            FDITickModel tickModel)
        {
            _model = model;
            _tickModel = tickModel;
        }

        // === 公开方法 ===
        public void Tick(float logicTime, float realTime)
        {
            // 生成快照
            Capture();
            
            // 实际逻辑
            TickScopeRegular(logicTime, realTime);
        }
        public void LateTick(float logicTime, float realTime)
        {
            // 实际逻辑
            LateTickScopeRegular(logicTime, realTime);
        }
        
        // === 内联方法 ===
        private void Capture()
        {
            // 变化检测
            if (_model.SortVersion == _tickModel.SortCaptureVersion) return;

            // 容量检测
            if (_tickModel.Scopes.Length < _model.ScopeSort.Count)
            {
                _tickModel.Scopes = new ScopeRecord[_model.ScopeSort.Count];
            }
            
            // 内容快照
            _model.ScopeSort.CopyTo(_tickModel.Scopes,0);
            _tickModel.ScopeCount = _model.ScopeSort.Count;
            
            // 同步版本
            _tickModel.SortCaptureVersion = _model.SortVersion;
        }
        private void TickScopeRegular(float logicTime, float realTime)
        {
            for (var i = 0; i <_tickModel.ScopeCount; i++)
            {
                var scope = _tickModel.Scopes[i];
                if(!scope.Valid) continue;
                
                var tickable = scope.Tickable;
                for (var j = 0; j < tickable.Count; j++)
                {
                    var tick = tickable[j];
                    tick.Tick(logicTime, realTime);
                }
            }
        }
        private void LateTickScopeRegular(float logicTime, float realTime)
        {
            for (var i = 0; i <_tickModel.ScopeCount; i++)
            {
                var scope = _tickModel.Scopes[i];
                if(!scope.Valid) continue;
                
                var lateTickable = scope.LateTickable;
                for (var j = 0; j < lateTickable.Count; j++)
                {
                    var lateTick = lateTickable[j];
                    lateTick.LateTick(logicTime, realTime);
                }
            }
        }
    }
}