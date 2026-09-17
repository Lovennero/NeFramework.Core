namespace Framework.Core.FrameDI
{
    public interface ITickable
    {
        void Tick(float logicTime, float realTime);
    }
}