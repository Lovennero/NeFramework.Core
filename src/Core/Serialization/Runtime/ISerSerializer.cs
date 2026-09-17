using System.Threading.Tasks;

namespace Framework.Core.Serialization
{
    public interface ISerSerializer { }
    public interface ISerSerializer<T> : ISerSerializer
    {
        ValueTask<byte[]> Serialize(T obj); 
        ValueTask<T> Deserialize(byte[] data);
    }
}