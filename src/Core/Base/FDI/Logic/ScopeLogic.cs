
namespace Framework.Core.FrameDI
{
    internal sealed class ScopeLogic : IScopeLogic
    {
        private const string Tag = "Scope Logic";
        
        private readonly ScopeResolveOperator _spResolveOp;
        
        public ScopeLogic(ScopeResolveOperator spResolveOp)
        {
            _spResolveOp = spResolveOp;
        }

        public T Resolve<T>(string scopeName) where T : class
        {
            var type = typeof(T);
            var instance = _spResolveOp.Resolve(scopeName, type);
            return instance as T;
        }

        public void Inject(string scopeName, object instance)
        {
            var type = instance.GetType();
            _spResolveOp.Inject(scopeName, type, instance);
        }
    }
}