using System;

namespace Framework.Core.FrameDI
{
    [Flags]
    public enum ELifecycle
    {
        None = 0,
        Startable = 1,
        Tickable = 2,
        LateTickable = 4,
        Releasable = 8
    }
}