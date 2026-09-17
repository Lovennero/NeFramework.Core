using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    internal sealed class EntityLifeLogic:IEntityLifeLogic
    {
        private const string Tag =  "Entity Lifecycle Logic";
        
        // === 实例数据 ===
        private readonly EntityModel _model;
        
        // === 辅助方法 ===
        private readonly IEntityProvider _provider;
        private readonly IEntityLifeHelper _lifeLifeHelper;
        private readonly IEntityPoolHelper _poolHelper;
        
        // === 原子操作 ===
        private readonly EntityRecycleOperator _recycleOp;
        
        public EntityLifeLogic(
            EntityModel model,
            IEntityProvider provider,
            IEntityLifeHelper lifeHelper,
            IEntityPoolHelper poolHelper,
            EntityRecycleOperator recycleOp)
        {
            _model = model;

            _provider = provider;
            _lifeLifeHelper = lifeHelper;
            _poolHelper = poolHelper;
            
            _recycleOp = recycleOp;
        }
        
        public async ValueTask<EntityRecord> AcquireEntity(string entityName, string groupName, string assetPath, int priority = 0, CancellationToken ct = default)
        {
            var groups = _model.Groups;
            if (!groups.TryGetValue(groupName,out var group) || !group.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} does not exist.");
            }
            
            // 创建实体缓存
            _model.SerialID++;
            var record = new EntityRecord(_model.SerialID, entityName, groupName, assetPath, priority);
            record.LifePhase = ELifePhase.Acquiring;
            record.LifeTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            // 注册实体缓存
            _model.Records.Add(record.ID, record);
            _model.Groups[record.GroupName].Members.Add(record.ID);
            var entitySort = _model.GroupEntitySorts[record.GroupName];
            entitySort.Add(record);
            entitySort.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            
            // 加载实体资源
            var asset = await _provider.LoadAssetAsync(assetPath);
            if (asset == null)
            {
                throw new InvalidOperationException($"[{Tag}]: Asset could not be loaded.");
            }
            
            // 创建实体池
            if (!_model.Pools.TryGetValue(assetPath, out var poolRecord))
            {
                poolRecord = new EntityPoolRecord(assetPath);
                _model.Pools.Add(assetPath, poolRecord);
                _model.PoolSorts.Add(poolRecord);
                _poolHelper.InitPool(assetPath, asset);
            }
            
            // 创建实体对象
            if (!await _lifeLifeHelper.Acquire(record, group.ScopeName, ct))
            {
                throw new InvalidOperationException($"[{Tag}]: Acquire failure.");
            }
            poolRecord.LastUseTime = -1;
            poolRecord.PoolRef++;
            
            // 执行获取周期
            await record.Entity.OnAcquire();
            
            // 标记完成加载
            record.LifePhase = ELifePhase.Active;
            record.LifeTcs.SetResult(true);
            
            return record;
        }

        public async ValueTask RecycleEntity(int id)
        {
            var records = _model.Records;

            if (!records.TryGetValue(id, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]:entity record (ID:{id}) does not exist.");
            }
            
            await _recycleOp.Execute(record);
        }
    }
}