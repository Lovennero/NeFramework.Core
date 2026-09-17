using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Asset
{
    public interface IAssetService
    {
        ValueTask<IAssetHandle<T>[]> LoadAllAssetAsync<T>(IReadOnlyList<string> path, CancellationToken ct = default, IProgress<float> progress = null) where T : class;

        ValueTask<IAssetHandle<T>> LoadAssetAsync<T>(string path, CancellationToken ct = default, IProgress<float> progress = null) where T : class;

        IAssetHandle<T> LoadAsset<T>(string path) where T : class;

        void Unload<T>(string path) where T : class;

        ValueTask UnloadUnusedAssetAsync();
    }
}
