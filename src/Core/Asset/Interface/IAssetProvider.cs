using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Asset
{
    public interface IAssetProvider:IDisposable
    {
        void OnUpdate(float logicTime,float realTime);
        
        ValueTask<IAssetHandle<T>> LoadAsync<T>(string path, CancellationToken ct = default, IProgress<float> progress = null) where T : class;
        
        ValueTask<IAssetHandle<T>[]> LoadAllAsync<T>(IReadOnlyList<string> path, CancellationToken ct = default, IProgress<float> progress = null) where T : class;
        
        IAssetHandle<T> LoadAsset<T>(string path) where T : class;
        
        void Unload<T>(string path) where T : class;

        public ValueTask UnloadUnusedAsync();
    }
}
