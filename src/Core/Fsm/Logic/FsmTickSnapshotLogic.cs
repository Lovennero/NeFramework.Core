namespace Framework.Core.Fsm
{
    internal sealed class FsmTickSnapshotLogic:IFsmTickSnapshotLogic
    {
        private const string Tag = "Fsm Tick Snapshot Logic";
        
        private readonly FsmModel _model;
        private readonly FsmTickModel _tickModel;
        
        public FsmTickSnapshotLogic(FsmModel model,FsmTickModel tickModel)
        {
            _model = model;
            _tickModel = tickModel;
        }

        public void Capture()
        {
            var fsmCount = _model.FsmRecords.Count;
            if (_tickModel.Fsms.Length < fsmCount)
            {
                _tickModel.Fsms = new FsmRecord[fsmCount];
            }
            _model.FsmRecords.Values.CopyTo(_tickModel.Fsms, 0);
            _tickModel.FsmCount = fsmCount;
        }
    }
}