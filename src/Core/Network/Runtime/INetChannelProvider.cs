namespace Framework.Core.Network
{
    public interface INetChannelProvider
    {
        INetChannel CreateChannel(string channelKey, INetOptions options);
    }
}