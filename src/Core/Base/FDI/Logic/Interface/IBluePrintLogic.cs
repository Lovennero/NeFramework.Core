namespace Framework.Core.FrameDI
{
    public interface IBluePrintLogic
    {
        // === 实现类注册 ===
        RegisterRecord Register<TConcrete>(string bluePrintName,  ELifetime lifetime = ELifetime.Singleton) where TConcrete : class;
        void UnRegister<TConcrete>(string bluePrintName) where TConcrete : class;
        
        // === 生命周期注册 ===
        void RegisterEntryPoint<TConcrete>(string name) where TConcrete : class;
        void UnregisterEntryPoint<TConcrete>(string name) where TConcrete : class;
    }
}