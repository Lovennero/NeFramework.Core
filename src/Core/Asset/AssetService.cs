using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Framework.Core.FrameDI;
using Framework.Core.FrameLog;

namespace Framework.Core.Asset
{
    public sealed class AssetService : IAssetService, IStartable, ITickable
    {
        private const string Tag = "Asset Service";
        private readonly int _mainThreadId;
        
        private readonly IAssetProvider _assetProvider;

        public AssetService(IAssetProvider assetProvider)
        {
            _mainThreadId = Environment.CurrentManagedThreadId;
            _assetProvider = assetProvider;
        }
        
        public void Start()
        {
            FLog.LogNormal($"[{Tag}]:Service Init!");
        }

        public void Tick(float logicTime, float realTime)
        {
            _assetProvider.OnUpdate(logicTime,realTime);
        }
        
        public ValueTask<IAssetHandle<T>[]> LoadAllAssetAsync<T>(IReadOnlyList<string> path, CancellationToken ct = default, IProgress<float> progress = null) where T : class
        {
            AssertMainThread();
            return _assetProvider.LoadAllAsync<T>(path,ct,progress);
        }
        
        public ValueTask<IAssetHandle<T>> LoadAssetAsync<T>(string path, CancellationToken ct = default, IProgress<float> progress = null) where T : class
        {
            AssertMainThread();
            return _assetProvider.LoadAsync<T>(path, ct, progress);
        }
        
        public IAssetHandle<T> LoadAsset<T>(string path) where T : class
        {
            AssertMainThread();
            return _assetProvider.LoadAsset<T>(path);
        }

        public void Unload<T>(string path) where T : class
        {
            AssertMainThread();
            _assetProvider.Unload<T>(path);
        }

        public ValueTask UnloadUnusedAssetAsync()
        {
            AssertMainThread();
            return _assetProvider.UnloadUnusedAsync();
        }

        private void AssertMainThread()
        {
            if (Environment.CurrentManagedThreadId == _mainThreadId) return;

            throw new InvalidOperationException($"[{Tag}]:Asset operations must run on the main thread.");
        }
    }
}
