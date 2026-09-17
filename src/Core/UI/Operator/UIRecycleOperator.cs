
using System.Threading.Tasks;
using Framework.Core.FrameTime;

namespace Framework.Core.UI
{
    internal sealed class UIRecycleOperator
    {
        private readonly UIModel _model;
        private readonly IUILifeHelper _lifeHelper;
        private readonly IUIHelper _helper;

        public UIRecycleOperator(UIModel model,IUILifeHelper lifeHelper, IUIHelper helper)
        {
            _model = model;
            _lifeHelper = lifeHelper;
            _helper = helper;
        }
        
        public async ValueTask Execute(UIRecord record)
        {
            if (!record.IsValid) return;
            record.IsValid = false;

            //池管理
            PoolRecycleCheck(record);
            
            //栈管理
            StackRecycleCheck(record);
            
            //显示管理
            await VisibleRecycleCheck(record);
            
            //真移除
            _lifeHelper.Recycle(record);
            record.UI.OnRecycle();
            
            _model.GroupsUIsSort[record.GroupName].Remove(record);
            _model.Groups[record.GroupName].GroupMembers.Remove(record.ID);
            _model.Records.Remove(record.ID);
        }
        
        // === 内联方法 ===
        private void PoolRecycleCheck(UIRecord record)
        {
            var poolRecord = _model.Pools[record.AssetPath];
            poolRecord.PoolRef--;
            if (poolRecord.PoolRef == 0)
            {
                poolRecord.LastUseTime = FTime.GetRealRuntime();
                _model.PoolsSort.Sort((a,b)=>a.LastUseTime.CompareTo(b.LastUseTime));
            }
        }

        private void StackRecycleCheck(UIRecord record)
        {
            var group = _model.Groups[record.GroupName];
            var stack = group.Stack;
            var recordIndex = stack.IndexOf(record.ID);
            if (recordIndex < 0) return;
            
            var wasTopUI = recordIndex == stack.Count - 1;
            stack.RemoveAt(recordIndex);
            
            if(!group.IsValid) return;
            if (!wasTopUI || stack.Count <= 0) return;
            var topUI = stack[^1];
            _model.Records[topUI].UI.OnRecycle();
        }

        private async ValueTask VisibleRecycleCheck(UIRecord record)
        {
            if(!record.IsShown) return;
            await record.UI.OnHide();
            _helper.HideUI(record.ID);
            record.IsShown = false;
        }
    }
}