namespace Framework.Core.UI
{
    internal class UIAcquireOperator
    {
        private readonly UIModel _model;

        public UIAcquireOperator(UIModel model)
        {
            _model = model;
        }

        public void Execute(UIRecord record)
        {
            var poolRecord = _model.Pools[record.AssetPath];
            poolRecord.LastUseTime = -1;
            poolRecord.PoolRef++;
            
            _model.Records.Add(record.ID, record);
            _model.Groups[record.GroupName].GroupMembers.Add(record.ID);

            var entitySort = _model.GroupsUIsSort[record.GroupName];
            entitySort.Add(record);
            entitySort.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            
            record.UI.OnAcquire();
            record.IsValid = true;
        }
    }
}