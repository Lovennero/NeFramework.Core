using System.Collections.Generic;

namespace Framework.Core.Sound
{
    internal sealed class SoundGroupAddOperator
    {
        private readonly SoundModel _model;

        public SoundGroupAddOperator(SoundModel model)
        {
            _model = model;
        }

        public void Execute(SoundGroupRecord record)
        {
            _model.GroupRecords.Add(record.GroupName, record);
            _model.GroupSort.Add(record);
            _model.GroupSoundSorts.Add(record.GroupName, new List<SoundRecord>());
            _model.GroupSort.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
    }
}
