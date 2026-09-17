using System.Threading.Tasks;

namespace Framework.Core.Serialization
{
    public interface ISerProcessor
    {
        ValueTask<byte[]> Process(byte[] data);
        ValueTask<byte[]> ReverseProcess(byte[] data);
    }
}