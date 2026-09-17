using System;

namespace Framework.Core.FrameDI
{
    internal sealed class BluePrintLifeLogic : IBluePrintLifeLogic
    {
        private const string Tag = "Blue Print Lifecycle Logic";

        private readonly FDIModel _model;
        
        public BluePrintLifeLogic(FDIModel model)
        {
            _model = model;
        }
        
        public BluePrintRecord CreateBluePrint(string bluePrintName)
        {
            var bpRecords = _model.BluePrintRecords;
            if (bpRecords.TryGetValue(bluePrintName, out var bpRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: BluePrint:{bluePrintName} already exists.");
            }
            
            bpRecord = new BluePrintRecord(bluePrintName);
            bpRecords.Add(bluePrintName, bpRecord);
            bpRecord.Valid = true;
            
            return bpRecord;
        }

        public void ReleaseBluePrint(string bluePrintName)
        {
            var bpRecords = _model.BluePrintRecords;
            var rtRecords = _model.RegisterRecords;
            if (!bpRecords.TryGetValue(bluePrintName, out var bpRecord) || !bpRecord.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: BluePrint:{bluePrintName} does not exists.");
            }
            
            // 手动移除，移除注册关系
            bpRecord.Valid = false;
            bpRecords.Remove(bpRecord.Name);

            foreach (var rtID in bpRecord.RtMap.Values)
            {
                rtRecords.Remove(rtID);
            }
            bpRecord.RtMap.Clear();        
        }
    }
}