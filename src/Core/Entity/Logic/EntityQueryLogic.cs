using System;

namespace Framework.Core.Entity
{
    internal sealed class EntityQueryLogic:IEntityQueryLogic
    {
        private const string Tag = "Entity Query Logic";
        
        private readonly EntityModel _model;

        public EntityQueryLogic(EntityModel model)
        {
            _model = model;
        }
        
        public bool HasGroup(string groupName)
        {
            var groups = _model.Groups;
            return groups.TryGetValue(groupName, out var group) && group.Valid;
        }

        public EntityGroupRecord GetGroup(string groupName)
        {
            var groups = _model.Groups;
            if (!groups.TryGetValue(groupName, out var groupRecord) || !groupRecord.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} does not exist.");
            }
            return groupRecord;
        }

        public bool HasEntity(int id)
        {
            var records = _model.Records;
            return records.TryGetValue(id, out var record) && record.LifePhase == ELifePhase.Active;
        }

        public EntityRecord GetEntity(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record) || record.LifePhase != ELifePhase.Active)
            {
                throw new InvalidOperationException($"[{Tag}]:Entity (ID:{id}) does not exist.");
            }
            return record;
        }
    }
}