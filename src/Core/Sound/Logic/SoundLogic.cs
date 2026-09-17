using System;
using System.Threading.Tasks;

namespace Framework.Core.Sound
{
    internal sealed class SoundLogic:ISoundLogic
    {
        private const string Tag = "Sound ControlLogic";
        
        private readonly SoundModel _model;
        private readonly SoundRecycleOperator _recycleOperator;
        
        public SoundLogic(
            SoundModel model,
            SoundRecycleOperator recycleOperator)
        {
            _model = model;
            _recycleOperator = recycleOperator;
        }

        public ValueTask<bool> Play(int id ,float fadeTime)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            return soundRecord.Sound.Play(fadeTime);
        }

        public async ValueTask<bool> Stop(int id ,float fadeTime)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            
            await soundRecord.Sound.Stop(fadeTime);
            
            _recycleOperator.Execute(soundRecord);
            
            return true;
        }

        public ValueTask<bool> Resume(int id ,float fadeTime)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            return soundRecord.Sound.Resume(fadeTime);
        }
        
        public ValueTask<bool> Pause(int id ,float fadeTime)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            return soundRecord.Sound.Pause(fadeTime);
        }

        public float GetVolume(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            
            return soundRecord.SoundParams.Volume;
        }
        
        public void SetVolume(int id, float volume)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            soundRecord.SoundParams.Volume = volume;
            soundRecord.Sound.RefreshParams();
        }

        public bool GetMute(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            return soundRecord.SoundParams.Mute;
        }
        
        public void SetMute(int id, bool mute)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            soundRecord.SoundParams.Mute = mute;
            soundRecord.Sound.RefreshParams();
        }
        
        public float GetPitch(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            return soundRecord.SoundParams.Pitch;
        }
        
        public void SetPitch(int id, float pitch)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            soundRecord.SoundParams.Pitch = pitch;
            soundRecord.Sound.RefreshParams();
        }

        public bool Validity(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record)) return false;
            return record.IsValid;
        }

        public ISound GetRawSound(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var soundRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Sound (ID:{id}) does not exist.");
            }
            return soundRecord.Sound;
        }
    }
}