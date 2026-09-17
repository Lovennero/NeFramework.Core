using System.Threading.Tasks;

namespace Framework.Core.Event
{
    public delegate ValueTask EventHandlerAsync<in T>(T message) where T : IMsg;
}