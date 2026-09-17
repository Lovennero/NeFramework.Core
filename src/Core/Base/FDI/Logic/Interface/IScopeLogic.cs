namespace Framework.Core.FrameDI
{
    public interface IScopeLogic
    { 
        T Resolve<T>(string scopeName) where T : class;
        void Inject(string scopeName, object instance);
    }
}