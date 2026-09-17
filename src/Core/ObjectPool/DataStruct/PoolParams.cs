namespace Framework.Core.ObjectPool
{
    public struct PoolParams
    {
        public int Capacity;
        public float AutoReleaseInterval;
        public float ExpirationTime;
        public OverStrategy Strategy;
    }
}
