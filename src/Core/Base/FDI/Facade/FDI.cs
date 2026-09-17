using System;
using Framework.Core.FrameLog;

namespace Framework.Core.FrameDI
{
    public static class FDI
    {
        private const string Tag = "Frame DI";

        // === 核心逻辑 ===
        private static IBluePrintLifeLogic _bluePrintLifeLogic;
        private static IBluePrintLogic _bluePrintLogic;
        private static IRegisterLogic _registerLogic;
        private static IScopeLifeLogic _scopeLifeLogic;
        private static IScopeLogic _scopeLogic;
        private static IScopeQueryLogic _scopeQueryLogic;
        private static IScopeTickLogic _scopeTickLogic;

        
        // === 运行状态 ===
        private static int _mainThreadID;
        private static bool Valid { get; set; }

        // === 框架周期 ===
        public static void Init()
        {
            if(Valid) return;
            
            var model = new FDIModel();
            var tickModel = new FDITickModel();

            var spResolveOp = new ScopeResolveOperator(model);
            
            _bluePrintLifeLogic = new BluePrintLifeLogic(model);
            _bluePrintLogic = new BluePrintLogic(model);
            _registerLogic = new RegisterLogic(model);
            _scopeLifeLogic = new ScopeLifeLogic(model, spResolveOp);
            _scopeLogic = new ScopeLogic(spResolveOp);
            _scopeQueryLogic = new ScopeQueryLogic(model);
            _scopeTickLogic = new ScopeTickLogic(model, tickModel);
            
            _mainThreadID = Environment.CurrentManagedThreadId;
            Valid = true;
            
            FLog.LogNormal($"[{Tag}] Utility Init!");
        }
        public static void Release()
        {
            if (!Valid) return;

            _bluePrintLifeLogic = null;
            _bluePrintLogic     = null;
            _registerLogic      = null;
            _scopeLifeLogic     = null;
            _scopeLogic         = null;
            _scopeTickLogic    = null;
            
            Valid = false;
            _mainThreadID = -1;
            
            FLog.LogNormal($"[{Tag}] Utility Release!");
        }
        
        // === 架构轮询 ===
        public static void TicK(float logicTime, float realTime)
        {
            if(!Valid) return;
            _scopeTickLogic.Tick(logicTime, realTime);
        }
        public static void LateTick(float logicTime, float realTime)
        {
            if(!Valid) return;
            _scopeTickLogic.LateTick(logicTime, realTime);
        }
        
        // === 蓝图构建 ===
        public static IBluePrintHandle CreateBluePrint(string name)
        {
            if (!Valid) return null;
            EnsureMainThread();
            
            if (string.IsNullOrWhiteSpace(name))
            {
                FLog.LogError($"[{Tag}]: Name is null or empty.");
                return null;
            }
            
            var bpRecord = _bluePrintLifeLogic.CreateBluePrint(name);
            return new BluePrintHandle(bpRecord.Name, _bluePrintLogic, _registerLogic);
        }

        public static void ReleaseBluePrint(string name)
        {
            if (!Valid) return;

            EnsureMainThread();
            
            if (string.IsNullOrWhiteSpace(name))
            {
                FLog.LogError($"[{Tag}]: Name is null or empty.");
                return;
            }
            
            _bluePrintLifeLogic.ReleaseBluePrint(name);
        }
        
        // === 作用域创建 ===
        public static IScopeHandle CreateScope(string name, string parentName)
        {
            if (!Valid) return null;

            EnsureMainThread();
            
            if (string.IsNullOrWhiteSpace(name))
            {
                FLog.LogError($"[{Tag}]: Name is null or empty.");
                return null;
            }

            if (string.IsNullOrEmpty(parentName))
            {
                parentName = null;
            }
            
            var spRecord = _scopeLifeLogic.CreateScope(name, parentName);
            return new ScopeHandle(spRecord.Name, _scopeLogic);
        }
        public static void ReleaseScope(string name)
        {
            if (!Valid) return ;
            
            EnsureMainThread();
            
            if (string.IsNullOrWhiteSpace(name))
            {
                FLog.LogError($"[{Tag}]: Name is null or empty.");
                return;
            }
            _scopeLifeLogic.ReleaseScope(name);
        }
        
        // === 作用域查询 ===
        public static bool HasScope(string name)
        {
            if (!Valid) return false;

            EnsureMainThread();
            
            return _scopeQueryLogic.HasScope(name);
        }

        public static IScopeHandle GetScope(string name)
        {
            if (!Valid) return null;
            
            EnsureMainThread();

            var record = _scopeQueryLogic.GetScope(name);
            return new ScopeHandle(record.Name, _scopeLogic);
        }
        
        public static bool TryGetScope(string name, out IScopeHandle handle)
        {
            handle = null;
            
            if(!Valid) return false;
            
            EnsureMainThread();
            
            if (_scopeQueryLogic.TryGetScope(name, out ScopeRecord scopeRecord))
            {
                handle = new ScopeHandle(scopeRecord.Name, _scopeLogic);
                return true;
            }
            
            return false;
        }
        
        // === 内联方法 ===
        private static void EnsureMainThread()
        {
            if (Environment.CurrentManagedThreadId == _mainThreadID) return;
            throw new InvalidOperationException($"[{Tag}]:current thread is not the main thread.");
        }
    }
}