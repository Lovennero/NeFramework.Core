using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Persistence
{
    public interface IPersistenceProvider
    {
        ValueTask<byte[]> ReadAllBytesAsync(string path, CancellationToken ct = default);
        
        ValueTask WriteAllBytesAsync(string path, byte[] data, CancellationToken ct = default);
    }
}