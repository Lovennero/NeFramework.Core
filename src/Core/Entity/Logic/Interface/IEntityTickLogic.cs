namespace Framework.Core.Entity
{
    public interface IEntityTickLogic
    {
        public void Tick(float logicTime, float realTime);
    }
}