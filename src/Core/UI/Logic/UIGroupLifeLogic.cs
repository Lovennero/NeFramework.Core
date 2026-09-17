using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.UI
{
    internal sealed class UIGroupLifeLogic : IUIGroupLifeLogic
    {
        private const string Tag = "UI Group Lifecycle Logic";
        
        private readonly UIModel _model;
        private readonly IUIGroupLifeHelper _lifeHelper;
        private readonly UIRecycleOperator _recycleOp;
        
        public UIGroupLifeLogic(UIModel model, IUIGroupLifeHelper lifeHelper, UIRecycleOperator recycleOp)
        {
            _model = model;
            _lifeHelper = lifeHelper;
            _recycleOp = recycleOp;
        }
        
        public async ValueTask<UIGroupRecord> AddGroup(string groupName, int priority,  UILayer layer, IUIGroupParams groupParams, string scopeName = "Frame")
        {
            var groups = _model.Groups;
            var groupsSort =  _model.GroupsSort;
            var groupsUIsSor = _model.GroupsUIsSort;
            
            if (groups.TryGetValue(groupName, out var group))
            {
                throw new InvalidOperationException($"[{Tag}]:Group '{groupName}' has already been added.");
            }
            
            await _lifeHelper.CreateGroupObj(groupName, layer, groupParams);
            
            group = new UIGroupRecord(groupName, priority, scopeName);
            groups[groupName] = group;
            groupsUIsSor.Add(groupName,new List<UIRecord>());
            groupsSort.Add(group);
            groupsSort.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            
            group.IsValid = true;

            return group;
        }

        public async ValueTask RemoveGroup(string groupName)
        {
            var groups = _model.Groups;
            var groupsSort =  _model.GroupsSort;
            var groupsUIsSort = _model.GroupsUIsSort;
            
            if (!groups.TryGetValue(groupName,out var groupRecord) || !groupRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]:Group '{groupName}' has already been removed.");
            }
            
            groupRecord.IsValid = false;
            
            var snapshot = groupsUIsSort[groupName].ToArray();
            for (var i = snapshot.Length - 1; i >= 0; i--)
            {
                var uiRecord = snapshot[i];
                await _recycleOp.Execute(uiRecord);
            }
            
            _lifeHelper.DestroyGroupObj(groupName);
            
            groups.Remove(groupName);
            groupsSort.Remove(groupRecord);
            groupsUIsSort.Remove(groupName);
        }
    }
}