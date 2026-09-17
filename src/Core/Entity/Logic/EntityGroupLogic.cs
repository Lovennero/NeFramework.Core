using System;
using System.Collections.Generic;

namespace Framework.Core.Entity
{
    internal class EntityGroupLogic:IEntityGroupLogic
    {
        private const string Tag = "Entity Group Logic";
        
        private readonly EntityModel _model;
        
        public EntityGroupLogic(EntityModel model)
        {
            _model = model;
        }

        public List<EntityRecord> GetGroupEntities(string groupName)
        {
            var records = _model.Records;
            var groups = _model.Groups;
            if (!groups.TryGetValue(groupName, out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} Members infos does not exist.");
            }

            var members = groupRecord.Members;
            var list = new List<EntityRecord>(members.Count);
            foreach (var memberID in members)
            {
                if (!records.TryGetValue(memberID, out var record))
                {
                    throw new InvalidOperationException($"[{Tag}]:Entity (ID:{memberID}) does not exist.)");
                }
                
                list.Add(record);
            }
            
            //即时快照
            list.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            return list;
        }
    }
}