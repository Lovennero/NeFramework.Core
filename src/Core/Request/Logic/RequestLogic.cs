using System;
using System.Runtime.CompilerServices;

namespace Framework.Core.Request
{
    internal sealed class RequestLogic : IRequestLogic
    {
        private const string Tag = "Request Logic";

        private readonly RequestModel _model;
        private readonly RequestCancelOperator _cancelOp;
        
        public RequestLogic(RequestModel model, RequestCancelOperator cancelOp)
        {
            _model = model;
            _cancelOp = cancelOp;
        }

        public RequestState RequestState(string requestKey)
        {
            var records = _model.Records;
            if (!records.TryGetValue(requestKey, out var record)) return Request.RequestState.Invalid;
            return record.Request.State;
        }

        public RequestResponse RequestResponse(string requestKey)
        {
            var records = _model.Records;
            if (!records.TryGetValue(requestKey, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]: Request:{requestKey} does not exist.");
            }

            return record.Request.Response;
        }
        
        public void CancelRequest(string requestKey)
        {
            var records = _model.Records;
            if (!records.TryGetValue(requestKey, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]: Request:{requestKey} does not exist.");
            }
            
            _cancelOp.Execute(record);
        }
        
        public TaskAwaiter<bool> GetRequestAwaiter(string requestKey)
        {
            var records = _model.Records;
            if (!records.TryGetValue(requestKey, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]: Request:{requestKey} does not exist.");
            }

            return record.Request.GetAwaiter();
        }
    }
}