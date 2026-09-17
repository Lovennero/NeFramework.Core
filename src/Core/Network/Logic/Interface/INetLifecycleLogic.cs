using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Network
{
    public interface INetLifecycleLogic
    {
        NetChannelRecord AddNetChannel(string channelKey, INetOptions options, CancellationToken ct = default);
        ValueTask RemoveNetChannel(string channelKey, CancellationToken ct = default);
    }
}