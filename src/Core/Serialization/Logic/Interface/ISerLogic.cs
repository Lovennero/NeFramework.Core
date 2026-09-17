using System.Threading.Tasks;

namespace Framework.Core.Serialization
{
    public interface ISerLogic
    {
        ValueTask<byte[]> Serialize<T>(string configKey, T obj);
        ValueTask<T> Deserialize<T>(string configKey, byte[] data);
    }
}