namespace Framework.Core.Entity
{
    public class EntityPoolRecord
    {
        public string PoolName { get; }
        public int PoolRef { get; set; }
        public float LastUseTime { get; set; }
        public bool IsValid { get; set; }

        internal EntityPoolRecord(string poolName)
        {
            PoolName = poolName;
            PoolRef = 0;
            LastUseTime = -1;
            IsValid = true;
        }
    }
}