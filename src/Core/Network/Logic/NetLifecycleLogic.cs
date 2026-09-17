using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Network
{
    internal sealed class NetLifecycleLogic : INetLifecycleLogic
    {
        private const string Tag =  "Net Lifecycle Logic";

        private readonly NetModel _model;
        
        private readonly INetChannelProvider _channelProvider;
        
        public NetLifecycleLogic(NetModel model,INetChannelProvider channelProvider)
        {
            _model = model;
            _channelProvider = channelProvider;
        }

        public NetChannelRecord AddNetChannel(string channelKey, INetOptions options, CancellationToken ct = default)
        {
            var records = _model.NetChannels;
            if (records.TryGetValue(channelKey, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]: Channel: {channelKey} already exists.");
            }
            
            var channel = _channelProvider.CreateChannel(channelKey, options);
            
            //添加缓存
            record = new NetChannelRecord(channelKey, channel);
            records.Add(channelKey, record);
            record.IsValid = true;

            return record;
        }

        public async ValueTask RemoveNetChannel(string channelKey ,CancellationToken ct = default)
        {
            var records = _model.NetChannels;
            if (!records.TryGetValue(channelKey, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]: Channel: {channelKey} does not exist.");
            }
            
            await record.NetChannel.CloseAsync(ct);
            records.Remove(channelKey);
            record.IsValid = false;
        }
    }
}