using System;

namespace Framework.Core.Fsm
{
    internal class FsmQueryLogic : IFsmQueryLogic
    {
        private const string Tag = "Fsm Query Logic";
        
        private readonly FsmModel _model;
        
        public FsmQueryLogic(FsmModel model)
        {
            _model = model;
        }
        
        public bool HasFsm(string fsmName)
        {
            var fsmRecords = _model.FsmRecords;
            return fsmRecords.ContainsKey(fsmName);
        }

        public FsmRecord GetFsm(string fsmName)
        {
            var fsmRecords = _model.FsmRecords;
            if (!fsmRecords.TryGetValue(fsmName, out var fsmRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: fsm {fsmName} does not exist.");
            }
            return fsmRecord;
        }
    }
}