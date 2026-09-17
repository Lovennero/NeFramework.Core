using System;

namespace Framework.Core.FrameDI
{
    internal sealed class ScopeQueryLogic : IScopeQueryLogic
    {
        private const string Tag = "Scope Query Logic";

        private readonly FDIModel _model;
        
        public ScopeQueryLogic(FDIModel model)
        {
            _model = model;
        }

        public bool HasScope(string name)
        {
            var spRecords = _model.ScopeRecords;
            return spRecords.ContainsKey(name);
        }

        public ScopeRecord GetScope(string name)
        {
            var spRecords = _model.ScopeRecords;
            if (!spRecords.TryGetValue(name, out var spRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Scope:{name} does not exist");
            }
            return spRecord;
        }

        public bool TryGetScope(string name, out ScopeRecord scopeRecord)
        {
            var spRecords = _model.ScopeRecords;
            
            if (spRecords.TryGetValue(name, out var spRecord))
            {
                scopeRecord = spRecord;
                return true;
            }
            
            scopeRecord = null;
            return false;
        }
    }
}