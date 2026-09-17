using System;

namespace Framework.Core.UI
{
    internal sealed class UIQueryLogic : IUIQueryLogic
    {
        private const string Tag = "UI Query Logic";
        
        private readonly UIModel _uiModel;
        
        public UIQueryLogic(UIModel uiModel)
        {
            _uiModel = uiModel;
        }
        
        public bool HasUI(int serialID)
        {
            var records = _uiModel.Records; 
            return records.ContainsKey(serialID);
        }
        
        public UIRecord GetUI(int serialID)
        {
            var records = _uiModel.Records;
            if (!records.TryGetValue(serialID, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]: UI: {serialID} does not find.");
            }
            return record;
        }
    }
}