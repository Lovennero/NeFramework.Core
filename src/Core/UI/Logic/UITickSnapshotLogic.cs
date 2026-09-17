using System;

namespace Framework.Core.UI
{
    internal sealed class UITickSnapshotLogic:IUITickSnapshotLogic
    {
        private const string Tag = "UI Tick Snapshot Logic";
        
        private readonly UIModel _model;
        private readonly UITickModel _tickModel;
        
        public UITickSnapshotLogic(UIModel model, UITickModel tickModel)
        {
            _model = model;
            _tickModel = tickModel;
        }
        
        public void Capture()
        {
            // === 元数据快照 ===
            var groupSort = _model.GroupsSort;
            var groupCount = groupSort.Count;

            
            if (_tickModel.Groups.Length < groupCount)
            {
                _tickModel.Groups = new UIGroupRecord[groupCount];
                _tickModel.GroupUIs = new UIRecord[groupCount][];
                _tickModel.GroupUICounts = new int[groupCount];
            }
            
            groupSort.CopyTo(_tickModel.Groups, 0);
            _tickModel.GroupCount = groupCount;

            for (var i = 0; i < groupCount; i++)
            {
                var group = groupSort[i];
                var uis = _model.GroupsUIsSort[group.GroupName];
                var uiCount = uis.Count;
                

                if (_tickModel.GroupUIs[i] == null || _tickModel.GroupUIs[i].Length < uiCount)
                    _tickModel.GroupUIs[i] = new UIRecord[Math.Max(uiCount, 4)];

                uis.CopyTo(_tickModel.GroupUIs[i], 0);
                _tickModel.GroupUICounts[i] = uiCount;
            }

            // === 池数据快照 ===
            var poolSort = _model.PoolsSort;
            var poolCount = poolSort.Count;
            
            _tickModel.PoolCount = poolCount;
            
            if (_tickModel.Pools.Length < poolCount)
            {
                _tickModel.Pools = new UIPoolRecord[poolCount];
            }
            
            poolSort.CopyTo(_tickModel.Pools, 0);
        }
    }
}