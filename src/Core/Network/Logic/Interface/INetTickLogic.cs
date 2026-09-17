namespace Framework.Core.Network
{
    public interface INetTickLogic
    {
        void Tick(float logicTime, float realTime);
    }
}