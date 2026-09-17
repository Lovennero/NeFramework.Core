using System.Threading.Tasks;

namespace Framework.Core.Event
{
    public interface IEventLogic
    {
        ValueTask PublishSeq<T>(string eventKey, T msg) where T : IMsg;
        ValueTask PublishCon<T>(string eventKey, T msg) where T : IMsg;
    }
}