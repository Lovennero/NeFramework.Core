using System;

namespace Framework.Core.FrameDI
{
    public interface IRegisterLogic
    {
        void AddBind<T>(int rtID);
        void RemoveBind<T>(int rtID);
    }
}