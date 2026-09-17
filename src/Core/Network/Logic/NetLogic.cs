using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Network
{
    internal sealed class NetLogic : INetLogic
    {
        private const string Tag = "Net Logic";
        
        private readonly NetModel _netModel;
        
        public NetLogic(NetModel netModel)
        {
            _netModel = netModel;
        }

        public ValueTask<NetConnectResult> ConnectAsync(string channelKey, INetConnectConfig config, CancellationToken ct = default)
        {
            var channels = _netModel.NetChannels;
            if (!channels.TryGetValue(channelKey, out var channel) || !channel.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Channel:{channelKey} doesn't exist.");
            }
            
            return channel.NetChannel.ConnectAsync(config, ct);
        }

        public ValueTask<NetSendResult> SendAsync(string channelKey, byte[] data, CancellationToken ct = default)
        {
            var channels = _netModel.NetChannels;
            if (!channels.TryGetValue(channelKey, out var channel) || !channel.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Channel:{channelKey} doesn't exist.");
            }
            
            return channel.NetChannel.SendAsync(data, ct);
        }

        public ValueTask<NetReceiveResult> ReceiveAsync(string channelKey, CancellationToken ct = default)
        {
            var channels = _netModel.NetChannels;
            if (!channels.TryGetValue(channelKey, out var channel) || !channel.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Channel:{channelKey} doesn't exist.");
            }
            return channel.NetChannel.ReceiveAsync(ct);
        }
    }
}