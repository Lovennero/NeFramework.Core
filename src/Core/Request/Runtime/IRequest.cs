using System;
using System.Runtime.CompilerServices;

namespace Framework.Core.Request
{
    public interface IRequest: IDisposable
    {
        RequestState State { get; }
        RequestResponse Response { get; }
        
        void Start();
        void Tick(float logicTime, float realTime);
        void Cancel();
        
        TaskAwaiter<bool> GetAwaiter();
    }
}