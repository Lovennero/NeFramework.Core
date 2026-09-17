using System;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Fsm
{
    internal sealed class FsmLifeLogic : IFsmLifeLogic
    {
        private const string Tag = "Fsm LifeCycle Logic";

        private readonly FsmModel _model;
        private readonly FsmStateDestroyOperator _stateDestroyOp;

        public FsmLifeLogic(
            FsmModel model,
            FsmStateDestroyOperator stateDestroyOp)
        {
            _model = model;
            _stateDestroyOp = stateDestroyOp;
        }

        public FsmRecord CreateFsm(string fsmName, string scopeName)
        {
            var fsmRecords = _model.FsmRecords;
            if (fsmRecords.TryGetValue(fsmName, out var fsmRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: fsm {fsmName} already exists.");
            }

            fsmRecord = new FsmRecord(fsmName, scopeName);
            fsmRecords.Add(fsmName, fsmRecord);
            fsmRecord.IsValid = true;
            return fsmRecord;
        }
        public async ValueTask RemoveFsm(string fsmName)
        {
            var fsmRecords = _model.FsmRecords;
            var fsmStateRecords = _model.FsmStateRecords;
            if (!fsmRecords.TryGetValue(fsmName, out var fsmRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: fsm {fsmName} does not exist.");
            }

            fsmRecord.IsValid = false;

            var stateIDs = fsmRecord.States.Values.ToArray();
            foreach (var stateID in stateIDs)
            {
                var stateRecord = fsmStateRecords[stateID];
                await _stateDestroyOp.Execute(stateRecord);
            }

            fsmRecords.Remove(fsmName);
        }
    }
}