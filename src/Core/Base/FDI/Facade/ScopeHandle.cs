namespace Framework.Core.FrameDI
{
    internal sealed class ScopeHandle : IScopeHandle
    {
        // === 基础参数 ===
        public string ScopeName { get; }
        
        // === 核心逻辑 ===
        private IScopeLogic ScopeLogic { get; }
        
        public ScopeHandle(string scopeName, IScopeLogic scopeLogic)
        {
            // 基础参数
            ScopeName = scopeName;
            
            // 核心逻辑
            ScopeLogic = scopeLogic;
        }

        // === 解析方法 ===
        public T Resolve<T>() where T : class
        {
            return ScopeLogic.Resolve<T>(ScopeName);
        }
        
        // === 注入方法 ===
        public void Inject(object obj)
        {
            ScopeLogic.Inject(ScopeName, obj);
        }
    }
}