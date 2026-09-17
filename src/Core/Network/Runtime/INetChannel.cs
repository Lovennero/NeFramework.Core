using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Network
{
    public interface INetChannel
    {
        NetworkState State { get; }

        void Tick(float logicTime, float realTime);

        ValueTask<NetConnectResult> ConnectAsync(INetConnectConfig config, CancellationToken ct = default);
        ValueTask<NetSendResult> SendAsync(byte[] data, CancellationToken ct = default);
        ValueTask<NetReceiveResult> ReceiveAsync(CancellationToken ct = default);
        ValueTask CloseAsync(CancellationToken ct = default);
    }
}