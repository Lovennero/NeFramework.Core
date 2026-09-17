namespace Framework.Core.Request
{
    public interface IRequestTickLogic
    {
        void Tick(float logicTime, float realTime);
    }
}