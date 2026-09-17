namespace Framework.Core.ObjectPool
{
    public interface IObject
    {
        bool IsValid => true;

        void OnSpawn();

        void OnDespawn();

        void OnRelease();
    }
}