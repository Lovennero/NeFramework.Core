using System;
using System.Collections.Generic;

namespace Framework.Core.UI
{
    internal sealed class UIGroupLogic:IUIGroupLogic
    {
        private const string Tag = "UI Group Logic";
        
        private readonly UIModel _model;
        
        public UIGroupLogic(UIModel uiModel)
        {
            _model = uiModel;
        }
        
        public bool GroupValidity(string groupName)
        {
            var groups =  _model.Groups;
            if (!groups.TryGetValue(groupName, out var group)) return false;
            return group.IsValid;
        }

        public List<UIRecord> GetGroupUIs(string groupName)
        {
            var records = _model.Records;
            var groups = _model.Groups;
            if (!groups.TryGetValue(groupName, out var group) || !group.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} does not exist.");
            }

            var list = new List<UIRecord>();
            var members = new int[group.GroupMembers.Count];
            group.GroupMembers.CopyTo(members, 0);
            foreach (var recordID in members)
            {
                if (!records.TryGetValue(recordID, out var record) || !record.IsValid)
                {
                    throw new InvalidOperationException($"[{Tag}]: Record {recordID} does not exist.");
                }
                list.Add(record);
            }
            
            list.Sort((a,b) => a.Priority.CompareTo(b.Priority));
            return list;
        }

        public void PushUI(string groupName, int uiID)
        {
            var groups = _model.Groups;
            if (!groups.TryGetValue(groupName, out var group) || !group.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} does not exist.");
            }
            
            var records = _model.Records;
            if (!records.TryGetValue(uiID, out var record) || !record.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: UI: {uiID} does not exist.");
            }

            if (!group.GroupMembers.Contains(record.ID))
            {
                throw new InvalidOperationException($"[{Tag}]: UI: {uiID} is not the member of Group {groupName}.");
            }

            if (group.Stack.Count > 0)
            {
                var oldUI = group.Stack[^1];
                if (!records.TryGetValue(oldUI, out var oldRecord) || !oldRecord.IsValid)
                {
                    throw new InvalidOperationException($"[{Tag}]: UI: {oldUI} was removed from the stack illegally.");
                }
                oldRecord.UI.OnBlur();
            }
            
            group.Stack.Add(record.ID);
        }

        public UIRecord PopUI(string groupName)
        {
            var groups = _model.Groups;
            if (!groups.TryGetValue(groupName, out var group) || !group.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Group: {groupName} does not exist.");
            }

            if (group.Stack.Count <= 0) return null;
            
            var recordIndex = group.Stack.Count - 1;
            var recordID =  group.Stack[recordIndex];
            
            var records =  _model.Records;
            if (!records.TryGetValue(recordID, out var record) || !record.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: UI: {recordID} was removed illegally when ui in the stack.");
            }
            group.Stack.RemoveAt(recordIndex);

            if (group.Stack.Count <= 0) return record;
            
            var topUi =  group.Stack[^1];
            if (!records.TryGetValue(topUi, out var topRecord) || !topRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: UI: {topUi} was removed illegally when ui in the stack.");
            }
            topRecord.UI.OnFocus();

            return record;
        }
    }
}