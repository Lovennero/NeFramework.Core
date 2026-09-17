using System;
using System.Collections.Generic;

namespace Framework.Core.Request
{
    internal sealed class RequestGroupLifecycleLogic : IRequestGroupLifecycleLogic
    {
        private const string Tag =  "Request Group Lifecycle Logic";

        private readonly RequestModel _model;

        private readonly RequestCancelOperator _cancelOp;
        
        public RequestGroupLifecycleLogic(RequestModel model ,RequestCancelOperator cancelOp)
        {
            _model = model;
            _cancelOp = cancelOp;
        }

        public void CreateRequestGroup(string groupKey, int priority, RequestGroupConfig config)
        {
            var groups = _model.Groups;
            var groupsSort = _model.GroupsSort;
            var groupsWaiters = _model.GroupsWaiters;
            var groupsRunners = _model.GroupsRunners;
            var groupsRetriers = _model.GroupsRetriers;
            
            if (groups.TryGetValue(groupKey, out var group))
            {
                throw new InvalidOperationException($"[{Tag}]: Group:{groupKey} already exists.");
            }
            
            var groupSequence = _model.GroupSequence ++;
            group = new RequestGroupRecord(groupSequence, groupKey, priority, config);
            
            groups.Add(groupKey, group);
            groupsSort.Add(group);
            groupsWaiters.Add(groupKey, new SortedSet<RequestRecord>());
            groupsRunners.Add(groupKey, new List<RequestRecord>());
            groupsRetriers.Add(groupKey, new HashSet<RequestRecord>());
        }

        public void RemoveRequestGroup(string groupKey)
        {
            var records = _model.Records;
            var groups = _model.Groups;
            var groupsSort = _model.GroupsSort;
            var groupsWaiters = _model.GroupsWaiters;
            var groupsRunners = _model.GroupsRunners;
            var groupsRetriers = _model.GroupsRetriers;
            
            if (!groups.TryGetValue(groupKey, out var group))
            {
                throw new InvalidOperationException($"[{Tag}]: Group:{groupKey} does not exists.");
            }
            
            //简易快照
            var members = new string[group.Members.Count];
            group.Members.CopyTo(members, 0);
            
            foreach (var member in group.Members)
            {
                var record = records[member];
                _cancelOp.Execute(record);
                
                record.Request.Dispose();
                records.Remove(member);
                group.Members.Remove(member);
            }

            groups.Remove(groupKey);
            groupsSort.Remove(group);
            groupsWaiters.Remove(groupKey);
            groupsRunners.Remove(groupKey);
            groupsRetriers.Remove(groupKey);
        }
    }
}