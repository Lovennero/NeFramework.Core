namespace Framework.Core.FrameDI
{
    public interface ILateTickable
    {
        void LateTick(float logicTime, float realTime);
    }
}