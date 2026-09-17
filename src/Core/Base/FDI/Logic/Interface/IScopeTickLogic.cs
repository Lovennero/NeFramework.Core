namespace Framework.Core.FrameDI
{
    public interface IScopeTickLogic
    {
        public void Tick(float logicTime, float realTime);
        public void LateTick(float logicTime, float realTime);
    }
}