using System;

namespace Framework.Core.FrameLog
{
    public static class FLog
    {
        private const string Tag = "Frame Log";
        
        private static IFLogHelper _ifLogHelper;

        private static bool _initialized;
        
        public static void Init(IFLogHelper logHelper)
        {
            if(_initialized) return;
            
            if (logHelper == null)
            {
                throw new InvalidOperationException($"[{Tag}]: logHelper is null.");
            }
            _ifLogHelper = logHelper;
            
            _initialized = true;
            
            LogNormal($"[{Tag}]:Utility Init!");
        }

        public static void Release()
        {
            if(!_initialized) return;
            _initialized = false;
            
            LogNormal($"[{Tag}]:Utility Release!");
            
            _ifLogHelper = null;
        }
        
        public static void LogNormal(string message)
        {
            if (_ifLogHelper == null) return;
            _ifLogHelper.LogNormal(message);
        }

        public static void LogWarning(string message)
        {
            if (_ifLogHelper == null) return;
            _ifLogHelper.LogWarning(message);
        }
        
        public static void LogError(string message)
        {
            if (_ifLogHelper == null) return;
            _ifLogHelper.LogError(message);
        }
    }
}
