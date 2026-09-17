namespace Framework.Core.Request
{
    internal class RequestCancelOperator
    {
        private readonly RequestModel _model;
        
        public RequestCancelOperator(RequestModel model)
        {
            _model = model;
        }

        public void Execute(RequestRecord record)
        {
            var group = _model.Groups[record.GroupKey];
            var groupWaiters =  _model.GroupsWaiters[record.GroupKey];
            var groupRunners = _model.GroupsRunners[record.GroupKey];
            var groupRetriers = _model.GroupsRetriers[record.GroupKey];

            if (record.Request.State == RequestState.Retry)
            {
                record.Request.Cancel();
                groupRetriers.Remove(record);
                return;
            }
            
            if (record.Request.State == RequestState.Waiting)
            {
                record.Request.Cancel();
                groupWaiters.Remove(record);
                return;
            }
            
            if (record.Request.State == RequestState.Running)
            {
                record.Request.Cancel();
                groupRunners.Remove(record);

                group.Concurrent--;
            }
        }
    }
}