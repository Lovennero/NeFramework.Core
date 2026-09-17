namespace Framework.Core.Fsm
{
    public interface IFsmHelper
    {
        void InjectState<T>(string scopeName, T state) where T : class, IState;
    }
}