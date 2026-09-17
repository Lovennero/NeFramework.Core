using System.Runtime.CompilerServices;

namespace Framework.Core.Request
{
    public interface IRequestLogic
    {
        RequestState RequestState(string requestKey);
        RequestResponse RequestResponse(string requestKey);

        
        void CancelRequest(string requestKey);
        TaskAwaiter<bool> GetRequestAwaiter(string requestKey);
    }
}