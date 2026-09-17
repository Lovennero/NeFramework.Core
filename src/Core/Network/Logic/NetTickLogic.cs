using System.Threading;

namespace Framework.Core.Network
{
    internal sealed class NetTickLogic : INetTickLogic
    {
        private const string Tag =  "Net Tick Logic";
        
        private readonly NetModel _netModel;
        private readonly NetTickModel _netTickModel;
        
        public NetTickLogic(NetModel netModel, NetTickModel netTickModel)
        {
            _netModel = netModel;
            _netTickModel = netTickModel;
        }

        public void Tick(float logicTime, float realTime)
        {
            Capture();
            TickChannelsRegular(logicTime, realTime);
        }
        
        // === 内联方法 ===
        private void Capture()
        {
            var channels = _netModel.NetChannels;
            var channelsCount = channels.Count;

            if (_netTickModel.NetChannels.Length < channelsCount)
            {
                _netTickModel.NetChannels = new NetChannelRecord[channelsCount];
            }
            
            channels.Values.CopyTo(_netTickModel.NetChannels, 0);
            _netTickModel.ChannelCount = channelsCount;
        }

        private void TickChannelsRegular(float logicTime, float realTime)
        {
            var channels = _netTickModel.NetChannels;
            var channelsCount = _netTickModel.ChannelCount;
            for (var i = 0; i < channelsCount; i++)
            {
                var channelRecord = channels[i];
                var netChannel = channelRecord.NetChannel;
                netChannel.Tick(logicTime, realTime);
            }
        }
    }
}