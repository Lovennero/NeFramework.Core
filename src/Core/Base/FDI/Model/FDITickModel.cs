using System;

namespace Framework.Core.FrameDI
{
    public class FDITickModel
    {
        public ScopeRecord[] Scopes = Array.Empty<ScopeRecord>();
        public int ScopeCount;
        public int SortCaptureVersion;
    }
}