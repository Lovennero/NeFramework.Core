using System;
using System.Threading;
using System.Threading.Tasks;
using Framework.Core.FrameDI;
using Framework.Core.FrameLog;

namespace Framework.Core.Scene
{
    public class SceneService : ISceneService, IStartable
    {
        private const string Tag = "Scene Service";
        private readonly int _mainThreadId;

        private readonly ISceneProvider _sceneProvider;
        private readonly ISceneHelper _sceneHelper;
        
        public SceneService(ISceneProvider sceneProvider,ISceneHelper sceneHelper)
        {
            _mainThreadId = Environment.CurrentManagedThreadId;
            _sceneProvider = sceneProvider;
            _sceneHelper = sceneHelper;
        }

        public void Start()
        {
            FLog.LogNormal($"[{Tag}]:Service Init!");
        }
        
        public ValueTask<ISceneHandle> LoadAsync(string scenePath, SceneLoadMode mode = SceneLoadMode.Additive,
            CancellationToken ct = default)
        {
            AssertMainThread();
            return _sceneProvider.LoadAsync(scenePath, mode, ct);
        }

        public ValueTask UnloadAsync(string scenePath, CancellationToken ct = default)
        {
            AssertMainThread();
            return _sceneProvider.UnloadAsync(scenePath, ct);
        }

        public void SetSceneActive(string scenePath)
        {
            AssertMainThread();
            _sceneHelper.SetActiveScene(scenePath);
        }

        private void AssertMainThread()
        {
            if (Environment.CurrentManagedThreadId == _mainThreadId) return;

            throw new InvalidOperationException($"[{Tag}]:Scene operations must run on the main thread.");
        }
    }
}
