namespace Framework.Core.Network
{
    public class NetChannelRecord
    {
        // === 基本信息 ===
        public string ChannelName { get; }
        
        // === 运行参数 ===
        public bool IsValid { get; set; }
        public INetChannel NetChannel { get; }
        
        internal NetChannelRecord(
            string channelName, 
            INetChannel netChannel)
        {
            ChannelName = channelName;
            NetChannel = netChannel;
        }
    }
}