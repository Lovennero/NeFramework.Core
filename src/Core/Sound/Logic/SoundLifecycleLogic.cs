using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Sound
{
    internal sealed class SoundLifecycleLogic : ISoundLifecycleLogic
    {
        private const string Tag = "Sound Lifecycle Logic";

        private readonly SoundModel _model;
        
        private readonly ISoundLifecycleHelper _lifecycleHelper;
        private readonly ISoundProvider _provider;
        
        private readonly SoundRecycleOperator _recycleOp;
        private readonly SoundAcquireOperator _soundAcquireOp;

        public SoundLifecycleLogic(
            SoundModel model,
            ISoundLifecycleHelper lifecycleHelper,
            ISoundProvider provider,
            SoundRecycleOperator recycleOp,
            SoundAcquireOperator soundAcquireOp)
        {
            _model = model;
            
            _lifecycleHelper = lifecycleHelper;
            _provider = provider;
            
            _recycleOp = recycleOp;
            _soundAcquireOp = soundAcquireOp;
        }

        public async ValueTask<SoundRecord> AcquireSound(
            string soundName, string groupName, string assetPath,
            ISoundParams soundParams, int priority = 0,
            CancellationToken ct = default)
        {
            if (!_model.GroupRecords.TryGetValue(groupName, out var group))
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} does not exist.");
            }
            
            _model.SerialID++;
            var record = new SoundRecord(_model.SerialID, soundName, assetPath, priority, groupName, soundParams);

            if (!await _lifecycleHelper.Acquire(record, ct))
            {
                throw new InvalidOperationException($"[{Tag}]:Acquire failure.");
            }

            var soundAsset = await _provider.LoadSoundAsync(assetPath,ct);
            if (soundAsset == null)
            {
                _lifecycleHelper.Recycle(record);
                throw new InvalidOperationException($"[{Tag}]: Sound asset could not be loaded.");
            }
            
            _soundAcquireOp.Execute(record, soundAsset, soundParams, group.SoundParams);
            return record;
        }

        public void Recycle(int id)
        {
            if (!_model.Records.TryGetValue(id, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]:sound record (ID:{id}) does not exist.");
            }

            _recycleOp.Execute(record);
            _provider.UnloadSound(record.AssetPath);
        }
    }
}