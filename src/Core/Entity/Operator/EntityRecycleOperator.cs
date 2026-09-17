using System.Linq;
using System.Threading.Tasks;
using Framework.Core.FrameTime;

namespace Framework.Core.Entity
{
    internal sealed class EntityRecycleOperator
    {
        private readonly EntityModel _model;
        private readonly IEntityLifeHelper _lifeHelper;
        private readonly IEntityHelper _helper;

        public EntityRecycleOperator(
            EntityModel model, 
            IEntityLifeHelper lifeHelper, 
            IEntityHelper helper)
        {
            _model = model;
            _lifeHelper = lifeHelper;
            _helper = helper;
        }

        public async ValueTask Execute(EntityRecord record)
        {
            // 等待任务执行
            if (record.LifeTcs != null) await record.LifeTcs.Task;
            if (record.LifePhase == ELifePhase.None) return;
            if (record.LifePhase == ELifePhase.Dead) return;
            
            // 判断回收条件
            record.LifePhase = ELifePhase.Recycling;
            record.LifeTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            // 递归回收实体
            var children = record.ChildrenIDs.ToArray();
            foreach (var childId in children)
            {
                var child = _model.Records[childId];
                await Execute(child);
            }
            
            // 执行实体隐藏
            await HideEntity(record);

            // 执行回收周期
            await record.Entity.OnRecycle();
            
            // 移除实体对象
            _lifeHelper.Recycle(record);
            var poolRecord = _model.Pools[record.AssetPath];
            poolRecord.PoolRef--;
            
            // 移除池对象
            if (poolRecord.PoolRef == 0)
            {
                poolRecord.LastUseTime = FTime.GetRealRuntime();
                _model.PoolSorts.Sort((a,b)=>a.LastUseTime.CompareTo(b.LastUseTime));
            }
            
            // 移除父子对象
            if (record.ParentID != -1 && _model.Records.TryGetValue(record.ParentID, out var parentRecord))
            {
                parentRecord.ChildrenIDs.Remove(record.ID);
            }
            
            // 移除缓存对象
            _model.GroupEntitySorts[record.GroupName].Remove(record);
            _model.Records.Remove(record.ID);
            _model.Groups[record.GroupName].Members.Remove(record.ID);
            
            // 标记死亡阶段
            record.LifePhase = ELifePhase.Dead;
            record.LifeTcs.SetResult(true);
        }

        private async ValueTask HideEntity(EntityRecord record)
        {
            // 状态判断
            if (record.ShowTcs != null) await record.ShowTcs.Task;
            if (record.ShowPhase == EShowPhase.Hide) return;

            record.ShowPhase = EShowPhase.Hiding;
            record.ShowTcs = new TaskCompletionSource<bool>();
            
            foreach (var childrenID in record.ChildrenIDs)
            {
                var child = _model.Records[childrenID];
                await Execute(child);
            }

            await record.Entity.OnHide();
            _helper.HideEntity(record.ID);
            
            record.ShowPhase = EShowPhase.Hide;
            record.ShowTcs = new TaskCompletionSource<bool>();
        }
    }
}
