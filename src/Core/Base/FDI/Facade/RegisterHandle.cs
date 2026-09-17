using System;

namespace Framework.Core.FrameDI
{
    internal sealed class RegisterHandle : IRegisterHandle
    {
        // === 基础参数 ===
        public int ID { get; }
        public string Name { get; }
        public Type ImplType { get; }
        
        // === 核心逻辑 ===
        private IRegisterLogic Logic { get; }
        
        public RegisterHandle(
            int id,
            string name,
            Type implType, 
            IRegisterLogic logic)
        {
            ID = id;
            Name = name;
            
            ImplType = implType;
            Logic = logic;
        }
        
        // === 公开方法 ===
        public IRegisterHandle AddBind<TInterface>()
        {
            Logic.AddBind<TInterface>(ID);
            return this;
        }

        public IRegisterHandle RemoveBind<TInterface>()
        {
            Logic.RemoveBind<TInterface>(ID);
            return this;
        }
    }
}