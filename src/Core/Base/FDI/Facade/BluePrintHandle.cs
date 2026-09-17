namespace Framework.Core.FrameDI
{
    internal sealed class BluePrintHandle : IBluePrintHandle
    {
        // === 基础参数 ===
        public string Name { get; }
        
        // === 核心逻辑 ===
        private IBluePrintLogic BluePrintLogic { get; }
        private IRegisterLogic RegisterLogic { get; }
        
        public BluePrintHandle(string name ,IBluePrintLogic bluePrintLogic,  IRegisterLogic registerLogic)
        {
            // 基础参数
            Name = name;
            
            // 核心逻辑
            BluePrintLogic = bluePrintLogic;
            RegisterLogic = registerLogic;
        }

        // === 注册逻辑 ===
        public IRegisterHandle Register<T>(ELifetime lifetime = ELifetime.Singleton) where T : class
        {
            var record = BluePrintLogic.Register<T>(Name, lifetime);
            return new RegisterHandle(record.SerialID, record.Name, record.ImpType, RegisterLogic);
        }
        
        public void UnRegister<T>() where T : class
        {
            BluePrintLogic.UnRegister<T>(Name);
        }

        // === 周期逻辑 ===
        public void RegisterEntryPoint<T>() where T : class
        {
            BluePrintLogic.RegisterEntryPoint<T>(Name);
        }

        public void UnRegisterEntryPoint<T>() where T : class
        {
            BluePrintLogic.UnregisterEntryPoint<T>(Name);
        }
    }
}