using System;
using Framework.Core.FrameLog;

namespace Framework.Core.FrameTime
{
    public static class FTime
    {
        private const string Tag = "Frame Time";
        
        private static IFTimeHelper _timeHelper;

        private static bool _initialized;
        
        public static void Init(IFTimeHelper timeHelper)
        {
            if(_initialized) return;
            
            if (timeHelper == null)
            {
                throw new InvalidOperationException( $"[{Tag}]: timeHelper is null.");
            }
            _timeHelper = timeHelper;
            
            _initialized = true;
            
            FLog.LogNormal($"[{Tag}]:Utility Init!");
        }
        
        public static void Release()
        {
            if(!_initialized) return;
            _timeHelper = null;
            _initialized = false;
            
            FLog.LogNormal($"[{Tag}]:Utility Release!");
        }

        public static float GetRealRuntime()
        {
            if (_timeHelper == null) return -1f;
            return _timeHelper.GetRealRuntime();
        }

        public static float GetLogicTime()
        {
            if(_timeHelper == null) return -1f;
            return _timeHelper.GetLogicTime();
        }
    }
}
