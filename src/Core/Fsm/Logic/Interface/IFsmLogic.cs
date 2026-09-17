using System.Threading.Tasks;
using Framework.Core.FrameVariable;

namespace Framework.Core.Fsm
{
    public interface IFsmLogic
    {
        // === 状态增删 ===
        FsmStateRecord AddState<T>(string fsmKey) where T : class, IState, new();
        FsmStateRecord AddState<T>(string fsmKey, T state) where T : class, IState;
        
        ValueTask RemoveState<T>(string fsmKey) where T : class, IState;
        
        // === 状态周期 ===
        ValueTask StartState<T>(string fsmKey) where T : class, IState;
        ValueTask ChangeState<T>(string fsmKey) where T : class, IState;
        ValueTask StopState(string fsmKey);

        // === 参数查询 ===
        bool Validity(string groupName);
        float CurrentStateTime(string groupName);
        FsmStateRecord CurrentStateType(string fsmKey);
        
        // === 状态查询 ===
        bool HasState<T>(string fsmKey) where T : class, IState;
        
        // === 上下参数 ===
        bool HasContext(string fsmKey, string contextKey);
        FVariable<T> GetContext<T>(string fsmKey, string contextKey);
        bool GetContext<T>(string fsmKey, string contextKey, out FVariable<T> fVariable);
        void AddContext<T>(string fsmKey, string contextKey, FVariable<T> fVariable);
        bool RemoveContext(string fsmKey, string contextKey);
    }
}