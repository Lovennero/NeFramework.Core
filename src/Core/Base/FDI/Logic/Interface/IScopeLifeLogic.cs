namespace Framework.Core.FrameDI
{
    public interface IScopeLifeLogic
    {
        ScopeRecord CreateScope(string name, string parentName);
        void ReleaseScope(string scopeName);
    }
}