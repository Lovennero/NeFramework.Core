namespace Framework.Core.FrameLog
{
    public interface IFLogHelper
    {
        void LogNormal(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
