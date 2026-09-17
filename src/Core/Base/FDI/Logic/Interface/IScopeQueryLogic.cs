namespace Framework.Core.FrameDI
{
    public interface IScopeQueryLogic
    {
        bool HasScope(string name);
        ScopeRecord GetScope(string name);
        bool TryGetScope(string name, out ScopeRecord scopeRecord);
    }
}