namespace Framework.Core.Sound
{
    internal sealed class SoundGroupRemoveOperator
    {
        private readonly SoundModel _model;

        public SoundGroupRemoveOperator(SoundModel model)
        {
            _model = model;
        }

        public void Execute(SoundGroupRecord record)
        {
            _model.GroupRecords.Remove(record.GroupName);
            _model.GroupSort.Remove(record);
            _model.GroupSoundSorts.Remove(record.GroupName);
        }
    }
}
