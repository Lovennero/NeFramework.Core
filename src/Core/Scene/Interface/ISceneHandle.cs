using System;

namespace Framework.Core.Scene
{
    public interface ISceneHandle:IDisposable
    {
        string ScenePath { get; }
        bool IsDone { get; }
        float Progress { get; }
    }
}
