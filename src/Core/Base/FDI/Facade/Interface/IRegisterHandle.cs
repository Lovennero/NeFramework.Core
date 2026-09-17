using System;

namespace Framework.Core.FrameDI
{
    public interface IRegisterHandle
    {
        // === 基础参数 ===
        public int ID { get; }
        public string Name { get; }
        Type ImplType { get; }
        
        // === 公开方法 ===
        IRegisterHandle AddBind<TInterface>();
        IRegisterHandle RemoveBind<TInterface>();
    }
}