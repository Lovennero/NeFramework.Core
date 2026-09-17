using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    internal sealed class EntityGroupLifecycleLogic:IEntityGroupLifecycleLogic
    {
        private const string Tag = "Entity Group Logic";
        
        // === 通用数据 ===
        private readonly EntityModel _model;
        
        // === 原子操作 ===
        private readonly EntityRecycleOperator _recycleOp;
        
        public EntityGroupLifecycleLogic(EntityModel model, EntityRecycleOperator recycleOp)
        {
            _model = model;
            _recycleOp = recycleOp;
        }
        
        public EntityGroupRecord AddGroup(string groupName, int priority, string scopeName)
        {
            if (_model.Groups.TryGetValue(groupName,out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} already exists.");
            }
            
            groupRecord = new EntityGroupRecord(groupName, priority, scopeName);
            
            _model.Groups.Add(groupRecord.GroupName, groupRecord);
            _model.GroupSort.Add(groupRecord);
            _model.GroupEntitySorts.Add(groupRecord.GroupName, new List<EntityRecord>());
            _model.GroupSort.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            
            // 标记组别有效
            groupRecord.Valid = true;
            
            return groupRecord;
        }

        public async ValueTask RemoveGroup(string groupName)
        {
            if (!_model.Groups.TryGetValue(groupName, out var groupRecord) || !groupRecord.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} does not exist.");
            }

            // 标记组别无效
            groupRecord.Valid = false;
            
            var snapshot = _model.GroupEntitySorts[groupName].ToArray();
            for (var i = snapshot.Length - 1; i >= 0; i--)
            {
                var entityRecord = snapshot[i];
                await _recycleOp.Execute(entityRecord);
            }
            
            _model.Groups.Remove(groupRecord.GroupName);
            _model.GroupSort.Remove(groupRecord);
            _model.GroupEntitySorts.Remove(groupRecord.GroupName);
        }
    }
}