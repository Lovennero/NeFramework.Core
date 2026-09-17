namespace Framework.Core.Sound
{
    internal sealed class SoundTickLogic : ISoundTickLogic
    {
        private const string Tag = "Sound Tick Logic";

        private readonly SoundModel _model;
        private readonly SoundTickModel _snapshot;

        private readonly SoundRecycleOperator _operator;
        
        public SoundTickLogic(SoundModel model, SoundTickModel snapshot,  SoundRecycleOperator op)
        {
            _model = model;
            _snapshot = snapshot;
            
            _operator = op;
        }

        public void Tick(float logicTime, float realTime)
        {
            TickSoundRegular(logicTime, realTime);
        }

        private void TickSoundRegular(float logicTime, float realTime)
        {
            for (var i = 0; i < _snapshot.GroupCount; i++)
            {
                var sounds = _snapshot.GroupSounds[i];
                var count = _snapshot.GroupSoundCounts[i];

                for (var j = 0; j < count; j++)
                {
                    var record = sounds[j];
                    if (!_model.Records.ContainsKey(record.ID)) continue;
                    
                    record.Sound.OnUpdate(logicTime, realTime);
                    
                    if(record.Sound.AutoStop) _operator.Execute(record);
                }
            }
        }
    }
}