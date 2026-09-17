namespace Framework.Core.Fsm
{
    public interface IFsmQueryLogic
    {
        public bool HasFsm(string fsmName);
        public FsmRecord GetFsm(string fsmName);
    }
}