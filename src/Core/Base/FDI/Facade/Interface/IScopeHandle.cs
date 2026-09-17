namespace Framework.Core.FrameDI
{
    public interface IScopeHandle
    {
        // === 基本参数 ===
        public string ScopeName { get; }
        
        // === 解析方法 ===
        T Resolve<T>() where T : class;
        void Inject(object obj);
    }
}