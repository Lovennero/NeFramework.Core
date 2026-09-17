using System;

namespace Framework.Core.Entity
{
    internal sealed class EntityTickSnapshotLogic : IEntityTickSnapshotLogic
    {
        private const string Tag = "Entity Tick Snapshot Logic";

        
        private readonly EntityModel _model;
        private readonly EntityTickModel _tickModel;
        
        public EntityTickSnapshotLogic(EntityModel model, EntityTickModel tickModel)
        {
            _model = model;
            _tickModel = tickModel;
        }
        
        public void Capture()
        {
            // === 元数据快照 ===
            var groupSort = _model.GroupSort;
            var groupCount = groupSort.Count;
            
            if (_tickModel.Groups.Length < groupCount)
            {
                _tickModel.Groups = new EntityGroupRecord[groupCount];
                _tickModel.GroupEntities = new EntityRecord[groupCount][];
                _tickModel.GroupEntityCounts = new int[groupCount];
            }
            
            // === 组数据快照 ===
            groupSort.CopyTo(_tickModel.Groups,0);
            _tickModel.GroupCount = groupCount;
            
            for (var i = 0; i < groupCount; i++)
            {
                var group = groupSort[i];

                var entities = _model.GroupEntitySorts[group.GroupName];
                var entityCount = entities.Count;
                
                if (_tickModel.GroupEntities[i] == null || _tickModel.GroupEntities[i].Length < entityCount)
                    _tickModel.GroupEntities[i] = new EntityRecord[Math.Max(entityCount, 4)];

                _tickModel.GroupEntityCounts[i] = entityCount;
                entities.CopyTo(_tickModel.GroupEntities[i], 0);
            }

            // === 池数据快照 ===
            var poolSort = _model.PoolSorts;
            var poolCount = poolSort.Count;
            
            if (_tickModel.Pools.Length < poolCount)
            {
                _tickModel.Pools = new EntityPoolRecord[poolCount];
            }
            
            _tickModel.PoolCount = poolCount;
            poolSort.CopyTo(_tickModel.Pools, 0);
        }
    }
}