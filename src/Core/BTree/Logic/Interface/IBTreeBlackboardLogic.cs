namespace Framework.Core.BTree
{
    public interface IBTreeBlackboardLogic
    {
        void SetValue<T>(string key, string bbKey, T value);
        public bool RemoveValue(string key, string bbKey);

        bool HasValue<T>(string key, string bbKey);
        public T GetValue<T>(string key, string bbKey);
        public bool TryGetValue<T>(string key, string bbKey, out T value);
    }
}