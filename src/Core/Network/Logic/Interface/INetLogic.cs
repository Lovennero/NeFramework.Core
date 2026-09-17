using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Network
{
    public interface INetLogic
    {
        ValueTask<NetConnectResult> ConnectAsync(string channelKey, INetConnectConfig config, CancellationToken ct = default);
        ValueTask<NetSendResult> SendAsync(string channelKey, byte[] data, CancellationToken ct = default);
        ValueTask<NetReceiveResult> ReceiveAsync(string channelKey, CancellationToken ct = default);
    }
}