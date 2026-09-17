using System;

namespace Framework.Core.Sound
{
    internal sealed class SoundTickSnapshotLogic : ISoundTickSnapshotLogic
    {
        private readonly SoundModel _model;
        private readonly SoundTickModel _tickModel;
        
        public SoundTickSnapshotLogic(SoundModel model, SoundTickModel tickModel)
        {
            _model = model;
            _tickModel = tickModel;
        }
        
        public void Capture()
        {
            var groupSort = _model.GroupSort;
            var groupCount = groupSort.Count;


            if (_tickModel.Groups.Length < groupCount)
            {
                _tickModel.Groups = new SoundGroupRecord[groupCount];
                _tickModel.GroupSounds = new SoundRecord[groupCount][];
                _tickModel.GroupSoundCounts = new int[groupCount];
            }
            
            _tickModel.GroupCount = groupCount;
            groupSort.CopyTo(_tickModel.Groups, 0);
            
            for (var i = 0; i < groupCount; i++)
            {
                var group = groupSort[i];
                var sounds = _model.GroupSoundSorts[group.GroupName];
                var soundCount = sounds.Count;

                if (_tickModel.GroupSounds[i] == null || _tickModel.GroupSounds[i].Length < soundCount)
                    _tickModel.GroupSounds[i] = new SoundRecord[Math.Max(soundCount, 4)];

                sounds.CopyTo(_tickModel.GroupSounds[i], 0);
                _tickModel.GroupSoundCounts[i] = soundCount;
            }
        }
    }
}
