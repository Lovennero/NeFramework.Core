using System;

namespace Framework.Core.Sound
{
    internal sealed class SoundGroupLifecycleLogic : ISoundGroupLifecycleLogic
    {
        private const string Tag = "Sound Group Lifecycle Logic";

        private readonly SoundModel _model;
        private readonly SoundGroupAddOperator _soundGroupAddOp;
        private readonly SoundGroupRemoveOperator _soundGroupRemoveOp;
        private readonly SoundRecycleOperator _recycleOp;

        public SoundGroupLifecycleLogic(
            SoundModel model,
            SoundGroupAddOperator soundGroupAddOp,
            SoundGroupRemoveOperator soundGroupRemoveOp,
            SoundRecycleOperator recycleOp)
        {
            _model = model;
            _soundGroupAddOp = soundGroupAddOp;
            _soundGroupRemoveOp = soundGroupRemoveOp;
            _recycleOp = recycleOp;
        }

        public SoundGroupRecord AddGroup(string groupName, int priority, ISoundParams group)
        {
            if (_model.GroupRecords.ContainsKey(groupName))
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} already exists.");
            }

            var groupRecord = new SoundGroupRecord(groupName, priority, group);
            _soundGroupAddOp.Execute(groupRecord);
            return groupRecord;
        }

        public void RemoveGroup(string groupName)
        {
            if (!_model.GroupRecords.TryGetValue(groupName, out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} does not exist.");
            }

            var soundSort = _model.GroupSoundSorts[groupName];
            for (var i = soundSort.Count - 1; i >= 0; i--)
                _recycleOp.Execute(soundSort[i]);

            _soundGroupRemoveOp.Execute(groupRecord);
        }
    }
}