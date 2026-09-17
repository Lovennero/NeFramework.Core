using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Scene
{
    public interface ISceneService
    {
        ValueTask<ISceneHandle> LoadAsync(string scenePath, SceneLoadMode mode = SceneLoadMode.Additive,
            CancellationToken ct = default);

        ValueTask UnloadAsync(string scenePath, CancellationToken ct = default);
        
        void SetSceneActive(string scenePath);
    }
}
