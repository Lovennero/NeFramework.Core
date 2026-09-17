namespace Framework.Core.Sound
{
    internal sealed class SoundRecycleOperator
    {
        private readonly SoundModel _model;
        private readonly ISoundLifecycleHelper _helper;

        public SoundRecycleOperator(SoundModel model, ISoundLifecycleHelper helper)
        {
            _model = model;
            _helper = helper;
        }

        public void Execute(SoundRecord record)
        {
            if (record.IsValid)
            {
                record.Sound.OnRecycle();
                _helper.Recycle(record);
                record.IsValid = false;
            }
            
            _model.GroupSoundSorts[record.GroupName].Remove(record);
            _model.Records.Remove(record.ID);
            _model.GroupRecords[record.GroupName].Members.Remove(record.ID);
        }
    }
}