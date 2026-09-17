using System;

namespace Framework.Core.Network
{
    internal sealed class NetQueryLogic : INetQueryLogic
    {
        private const string Tag = "Net Query Logic";

        private readonly NetModel _netModel;
        
        public NetQueryLogic(NetModel netModel)
        {
            _netModel = netModel;
        }

        public bool HasChannel(string channelKey)
        {
            var channels = _netModel.NetChannels;
            return channels.ContainsKey(channelKey);
        }

        public NetChannelRecord GetChannel(string channelKey)
        {
            var channels = _netModel.NetChannels;
            if (!channels.TryGetValue(channelKey, out var channel))
            {
                throw new InvalidOperationException($"[{Tag}]: Channel: {channelKey} does not exist.");
            }
            return channel;
        }
    }
}