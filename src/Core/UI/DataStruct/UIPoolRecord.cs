namespace Framework.Core.UI
{
    public class UIPoolRecord
    {
        public string PoolName { get; }
        public int PoolRef { get; set; }
        public float LastUseTime { get; set; }
        public bool IsValid { get; set; }

        internal UIPoolRecord(string poolName)
        {
            PoolName = poolName;
            PoolRef = 0;
            LastUseTime = -1;
            IsValid = true;
        }
    }
}