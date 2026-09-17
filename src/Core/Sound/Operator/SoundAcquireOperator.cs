namespace Framework.Core.Sound
{
    internal sealed class SoundAcquireOperator
    {
        private readonly SoundModel _model;

        public SoundAcquireOperator(SoundModel model)
        {
            _model = model;
        }

        public void Execute(SoundRecord record,ISoundAsset soundAsset, ISoundParams soundParams, ISoundParams groupParams)
        {
            _model.Records.Add(record.ID, record);
            _model.GroupRecords[record.GroupName].Members.Add(record.ID);

            var soundSort = _model.GroupSoundSorts[record.GroupName];
            soundSort.Add(record);
            soundSort.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            
            record.Sound.OnAcquire(soundAsset, soundParams,groupParams);
            record.IsValid = true;
        }
    }
}
