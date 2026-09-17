using System;
using System.Threading.Tasks;

namespace Framework.Core.UI
{
    internal sealed class UILogic:IUILogic
    {
        private const string Tag = "UI Logic";
        
        private readonly UIModel _model;
        private readonly UIShowOperator _showOp;
        private readonly UIHideOperator _hideOp;
        
        public UILogic(UIModel model,UIShowOperator showOp, UIHideOperator hideOp)
        {
            _model = model;
            _showOp = showOp;
            _hideOp = hideOp;
        }
        
        public bool Validity(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record)) return false;
            return record.IsValid;
        }

        public bool Visibility(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record) || !record.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]:UI(ID:{id}) not found.)");
            }
            return record.IsShown;
        }

        public async ValueTask ShowUI(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record) || !record.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]:UI(ID:{id}) not found.)");
            }
            
            await _showOp.Execute(record);
        }
        
        public async ValueTask HideUI(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record) || !record.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]:UI(ID:{id}) not found.)");
            }
            
            await _hideOp.Execute(record);
        }

        public IUI GetUI(int id)
        {
            var records = _model.Records;
            if (!records.TryGetValue(id, out var record) || !record.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]:UI(ID:{id}) not found.)");
            }

            return record.UI;
        }
    }
}