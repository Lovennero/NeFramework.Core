using System;

namespace Framework.Core.Request
{
    internal sealed class RequestLifecycleLogic:IRequestLifecycleLogic
    {
        private const string Tag = "Request Lifecycle Logic";
        
        private readonly RequestModel _model;
        
        private readonly IRequestLifecycleHelper _helper;
        
        private readonly RequestCancelOperator _cancelOp;
        
        public RequestLifecycleLogic(
            RequestModel model,
            IRequestLifecycleHelper helper,
            RequestCancelOperator cancelOp)
        {
            _model = model;
            _helper = helper;
            _cancelOp = cancelOp;
        }

        public RequestRecord AddRequest(string requestKey, string groupKey, int priority, RequestConfig config)
        {
            // === 检测工作 ===
            var records = _model.Records;
            var groups = _model.Groups;
            
            if (!groups.TryGetValue(groupKey, out var group))
            {
                throw new ArgumentException($"Group {groupKey} does not exist");
            }
            
            if (records.TryGetValue(requestKey, out var record))
            {
                throw new InvalidOperationException($"[{Tag}] Request key already exists.");
            }
            
            // === 逻辑处理 ===
            var sequence =  _model.Sequence++;
            var waiters = _model.GroupsWaiters[group.GroupKey];
            
            var request = _helper.AddRequest(requestKey, group.GroupConfig, config);
            record = new RequestRecord(requestKey,groupKey, priority, sequence, config, request);
            
            group.Members.Add(requestKey);
            records.Add(requestKey, record);
            waiters.Add(record);

            return record;
        }
        
        public void RemoveRequest(string requestKey)
        {
            // === 检测工作 ===
            var records = _model.Records;
            if (!records.TryGetValue(requestKey, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]: Record:{requestKey} does not exists.");
            }
            
            // === 逻辑处理 ===
            _cancelOp.Execute(record);
            
            _helper.RemoveRequest(requestKey);
            records.Remove(requestKey);
            
            var group = _model.Groups[record.GroupKey];
            group.Members.Remove(record.RequestKey);
        }
    }
}