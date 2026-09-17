namespace Framework.Core.Fsm
{
    public interface IFsmTickLogic
    {
        void Tick(float logicTime, float realTime);
    }
}