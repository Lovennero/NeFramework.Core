using System.Threading.Tasks;

namespace Framework.Core.Fsm
{
    internal sealed class FsmStateDestroyOperator
    {
        private readonly FsmModel _fsmModel;
        
        public FsmStateDestroyOperator(FsmModel fsmModel)
        {
            _fsmModel = fsmModel;
        }

        public async ValueTask Execute(FsmStateRecord stateRecord)
        {
            if(!stateRecord.IsValid) return;
            stateRecord.IsValid = false;
            
            var fsmRecords = _fsmModel.FsmRecords;
            var fsmStateRecords = _fsmModel.FsmStateRecords;
            var fsmRecord = fsmRecords[stateRecord.FsmKey];

            if (fsmRecord.CurrentState == stateRecord.SerialID)
            {
                fsmRecord.StateChangeTcs = new TaskCompletionSource<bool>();
                
                await stateRecord.State.OnExit();
                
                fsmRecord.CurrentState = -1;
                fsmRecord.CurrentStateTime = 0f;
                
                fsmRecord.StateChangeTcs.SetResult(true);
            }
            
            stateRecord.State.OnDestroy();
            fsmRecord.States.Remove(stateRecord.StateType);
            fsmStateRecords.Remove(stateRecord.SerialID);
        }
    }
}