using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.UI
{
    internal sealed class UILifeLogic:IUILifeLogic
    {
        private const string Tag = "UI Lifecycle Logic";
        
        // === 实例数据 ===
        private readonly UIModel _model;

        // === 辅助方法 ===
        private readonly IUIProvider _provider;
        private readonly IUILifeHelper _lifeHelper;
        private readonly IUIPoolHelper _poolHelper;
        
        // === 原子操作 ===
        private readonly UIAcquireOperator _acquireOp;
        private readonly UIRecycleOperator _recycleOp;
        
        public UILifeLogic(
            UIModel model,
            IUIProvider provider,
            IUILifeHelper lifeHelper,
            IUIPoolHelper poolHelper,
            UIAcquireOperator acquireOp,
            UIRecycleOperator recycleOp)
        {
            _model = model;
            _provider = provider;
            _lifeHelper = lifeHelper;
            _poolHelper = poolHelper;
            _acquireOp = acquireOp;
            _recycleOp = recycleOp;
        }
        
        public async ValueTask<UIRecord> AcquireUI(string groupName, string uiName, string assetPath, int priority = 0, CancellationToken ct = default)
        {
            var groups = _model.Groups;
            if (!groups.TryGetValue(groupName, out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} does not exist.");
            }

            _model.SerialID++;
            var record = new UIRecord(_model.SerialID, uiName, groupName, assetPath, priority);

            var asset = await _provider.LoadAssetAsync(assetPath);
            if (asset == null)
            {
                throw new InvalidOperationException($"[{Tag}]: Asset {assetPath} does not exist.");
            }
            
            if (!_model.Pools.TryGetValue(assetPath, out var pool))
            {
                pool = new UIPoolRecord(assetPath);
                _poolHelper.InitPool(assetPath, asset);
                _model.Pools.Add(pool.PoolName, pool);
                _model.PoolsSort.Add(pool);
            }
            
            if (!await _lifeHelper.Acquire(record, groupRecord.ScopeName, ct))
            {
                throw new InvalidOperationException($"[{Tag}]: Acquire failure.");
            }
            
            _acquireOp.Execute(record);
            return record;
        }

        public ValueTask RecycleUI(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]:entity record (ID:{id}) does not exist.");
            }
            
            return _recycleOp.Execute(record);
        }
    }
}