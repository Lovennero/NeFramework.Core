using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Persistence
{
    internal sealed class PersistenceLogic : IPersistenceLogic
    {
        private const string Tag = "Persistence Logic";
        
        private readonly IPersistenceProvider _provider;

        public PersistenceLogic(IPersistenceProvider provider)
        {
            _provider = provider;
        }
        
        public ValueTask<byte[]> Read(string path, CancellationToken ct = default)
        {
            return _provider.ReadAllBytesAsync(path, ct);
        }

        public ValueTask Write(string path, byte[] data, CancellationToken ct = default)
        { 
            return _provider.WriteAllBytesAsync(path, data, ct);
        }
    }
}