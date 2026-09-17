using System;
using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    internal sealed class EntityLogic:IEntityLogic
    {
        private const string Tag = "Entity Logic";
        
        private readonly EntityModel _model;
        private readonly IEntityHelper _entityHelper;
        

        public EntityLogic(EntityModel model, IEntityHelper entityHelper)
        {
            _model = model;
            _entityHelper = entityHelper;
        }

        public ELifePhase LifePhase(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record)) return ELifePhase.None;
            return record.LifePhase;
        }

        public EShowPhase ShowPhase(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record) || record.LifePhase != ELifePhase.Active)
            {
                throw new InvalidOperationException($"[{Tag}]:Record not found.");
            }
            return record.ShowPhase;
        }

        public async ValueTask ShowEntity(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record) || record.LifePhase != ELifePhase.Active)
            {
                throw new InvalidOperationException($"[{Tag}]:Record not found or invalid.");
            }

            if (record.ShowTcs != null) await record.ShowTcs.Task;
            if (record.ShowPhase == EShowPhase.Show) return;
            
            record.ShowPhase = EShowPhase.Showing;
            record.ShowTcs = new TaskCompletionSource<bool>();
            
            _entityHelper.ShowEntity(id);
            await record.Entity.OnShow();
            
            foreach (var childrenID in record.ChildrenIDs)
            {
                await ShowEntity(childrenID);
            }
            
            record.ShowPhase = EShowPhase.Show;
            record.ShowTcs.SetResult(true);
        }

        public async ValueTask HideEntity(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record) || record.LifePhase != ELifePhase.Active)
            {
                throw new InvalidOperationException($"[{Tag}]:Record not found.");
            }
            
            // 状态判断
            if (record.ShowTcs != null) await record.ShowTcs.Task;
            if (record.ShowPhase == EShowPhase.Hide) return;

            record.ShowPhase = EShowPhase.Hiding;
            record.ShowTcs = new TaskCompletionSource<bool>();
            
            foreach (var childrenID in record.ChildrenIDs)
            {
                await HideEntity(childrenID);
            }

            await record.Entity.OnHide();
            _entityHelper.HideEntity(record.ID);
            
            record.ShowPhase = EShowPhase.Hide;
            record.ShowTcs.SetResult(true);
        }

        public async ValueTask MountLogicEntity(int parentId, int childId)
        {
            var records = _model.Records;
            
            if (!records.TryGetValue(childId, out var child) || child.LifePhase != ELifePhase.Active)
            {
                throw new InvalidOperationException($"[{Tag}]:Record {childId} not found or invalid.");
            }

            if (!records.TryGetValue(parentId, out var parent) || parent.LifePhase != ELifePhase.Active)
            {
                throw new InvalidOperationException($"[{Tag}]:Parent {parentId} not found or invalid.");
            }
            
            if (WouldCreateCycle(parentId, childId))
            {
                throw new InvalidOperationException($"[{Tag}]:Setting parent {parentId} for child {childId} would create a cycle.");
            }
            
            if (child.ParentID == parentId && parent.ChildrenIDs.Contains(childId))
            {
                throw new InvalidOperationException($"[{Tag}]:Entity:{parentId} and Entity:{childId} have already established a parent-child relationship.");
            }
            
            //切断对象
            if (child.ParentID != -1)
            {
                if (!records.TryGetValue(child.ParentID, out var oldParent))
                {
                    throw new InvalidOperationException($"[{Tag}]:The original parent {child.ParentID} of the child  does not exist.");
                }
                
                oldParent.ChildrenIDs.Remove(childId);
            }
            
            parent.ChildrenIDs.Add(childId);
            child.ParentID = parentId;
            
            if (parent.ShowTcs != null) await parent.ShowTcs.Task;
            if (parent.ShowPhase == EShowPhase.Show)
            {
                await ShowEntity(childId);
                return;
            }
            if (parent.ShowPhase == EShowPhase.Hide)
            {
                await HideEntity(childId);
            }
        }

        public IEntity GetEntity(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record) || record.LifePhase != ELifePhase.Active)
            {
                throw new InvalidOperationException($"[{Tag}]:Record:{id} not found or invalid.");
            }
            return record.Entity;
        }

        // === 显影方法 ===
        private bool WouldCreateCycle(int parentId, int childId)
        {
            var records = _model.Records;
            var currentId = parentId;
            while (currentId != -1)
            {
                if (currentId == childId) return true;

                if (!records.TryGetValue(currentId, out var current) || current.LifePhase != ELifePhase.Active)
                {
                    throw new InvalidOperationException($"[{Tag}]:Record {currentId} not found while checking parent chain.");
                }

                currentId = current.ParentID;
            }
            
            return false;
        }
    }
}