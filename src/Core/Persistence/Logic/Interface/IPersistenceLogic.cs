using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Persistence
{
    public interface IPersistenceLogic
    {
        ValueTask<byte[]> Read(string path, CancellationToken ct = default);
        ValueTask Write(string path, byte[] data, CancellationToken ct = default);
    }
}