namespace Framework.Core.Fsm
{
    internal sealed class FsmTickLogic : IFsmTickLogic
    {
        private readonly FsmModel _model;
        private readonly FsmTickModel _tickModel;
        
        public FsmTickLogic(FsmModel model, FsmTickModel tickModel)
        {
            _model = model;
            _tickModel = tickModel;
        }

        public void Tick(float logicTime, float realTime)
        {
            TickFsmRegular(logicTime, realTime);
        }

        private void TickFsmRegular(float logicTime, float realTime)
        {
            var stateRecords = _model.FsmStateRecords;
            var fsmRecords = _tickModel.Fsms;
            var fsmCount = _tickModel.FsmCount;
            
            for (var i = 0; i < fsmCount; i++)
            {
                var fsmRecord = fsmRecords[i];
                if (!fsmRecord.IsValid) continue;
                if (fsmRecord.CurrentState == -1) continue;
                var tcs = fsmRecord.StateChangeTcs;
                if(!tcs.Task.IsCompleted) continue;
                
                var stateRecord = stateRecords[fsmRecord.CurrentState];
                fsmRecord.CurrentStateTime += logicTime;
                stateRecord.State.OnUpdate(logicTime, realTime);
            }
        }
    }
}