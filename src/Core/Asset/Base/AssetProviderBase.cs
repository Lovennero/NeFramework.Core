using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Asset
{
    public abstract class AssetProviderBase:IAssetProvider
    {
        protected bool Disposed;
        
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly CancellationTokenSource _disposeCts;
        
        protected AssetProviderBase(int maxConcurrent = 8)
        {
            _semaphoreSlim = new SemaphoreSlim(maxConcurrent, maxConcurrent);
            _disposeCts = new CancellationTokenSource();
        }
        
        public abstract void OnUpdate(float logicTime,float realTime);
        
        protected abstract ValueTask<IAssetHandle<T>> DoLoadAsync<T>(string path, CancellationToken ct) where T : class;
        
        protected abstract IAssetHandle<T> DoLoad<T>(string path) where T : class;
        
        public abstract void Unload<T>(string path) where T : class;

        public abstract ValueTask UnloadUnusedAsync();
        
        public async ValueTask<IAssetHandle<T>> LoadAsync<T>(string path, CancellationToken ct = default, IProgress<float> progress = null) where T : class
        {
            if (Disposed) return null;
            
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, _disposeCts.Token);
            await _semaphoreSlim.WaitAsync(linkedCts.Token);

            try
            {
                var handle = await DoLoadAsync<T>(path,linkedCts.Token);
                progress?.Report(1f);
                return handle;
            }
            finally
            {
                if(!Disposed) _semaphoreSlim.Release();
            }
        }
        
        public async ValueTask<IAssetHandle<T>[]> LoadAllAsync<T>(IReadOnlyList<string> path, CancellationToken ct = default, IProgress<float> progress = null) where T : class
        {
            if(Disposed) return null;
            
            if (path == null || path.Count == 0) return Array.Empty<IAssetHandle<T>>();

            var total = path.Count;
            var done = 0;
            var tasks = new Task<IAssetHandle<T>>[total];

            for (var i = 0; i < total; i++)
            {
                tasks[i] = LoadOne(path[i]).AsTask();
            }

            return await Task.WhenAll(tasks);

            async ValueTask<IAssetHandle<T>> LoadOne(string tmpPath)
            {
                var handle = await LoadAsync<T>(tmpPath, ct);
                var n = Interlocked.Increment(ref done);
                progress?.Report((float)n / total);
                return handle;
            }
        }
        
        public IAssetHandle<T> LoadAsset<T>(string path) where T : class
        {
            if(Disposed) return null;
            
            return DoLoad<T>(path);
        }
        
        public void Dispose()
        {
            if(Disposed) return;
            Disposed = true;
            
            _disposeCts.Cancel();
            _disposeCts.Dispose();
            _semaphoreSlim.Dispose();
        }
    }
}
