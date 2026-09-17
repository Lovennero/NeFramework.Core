namespace Framework.Core.FrameDI
{
    public interface IBluePrintHandle
    {
        // === 基础参数 ===
        string Name { get; }

        // === 注册逻辑 ===
        IRegisterHandle Register<T>(ELifetime lifetime = ELifetime.Singleton) where T : class;
        void UnRegister<T>() where T : class;
        
        // === 周期逻辑 ===
        void RegisterEntryPoint<T>() where T : class;
        void UnRegisterEntryPoint<T>() where T : class;
    }
}