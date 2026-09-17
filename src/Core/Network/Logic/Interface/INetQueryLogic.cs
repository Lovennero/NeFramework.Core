namespace Framework.Core.Network
{
    public interface INetQueryLogic
    {
        bool HasChannel(string channelKey);
        NetChannelRecord GetChannel(string channelKey);

    }
}