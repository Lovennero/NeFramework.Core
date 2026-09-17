using System;

namespace Framework.Core.Network
{
    internal sealed class NetTickModel
    {
        public NetChannelRecord[] NetChannels = Array.Empty<NetChannelRecord>();
        public int ChannelCount;
    }
}