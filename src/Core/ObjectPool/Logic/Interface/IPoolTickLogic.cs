namespace Framework.Core.ObjectPool
{
    public interface IPoolTickLogic
    {
        void Tick(float logicTime, float realTime);
    }
}