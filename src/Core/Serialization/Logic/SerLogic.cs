using System;
using System.Threading.Tasks;

namespace Framework.Core.Serialization
{
    internal sealed class SerLogic : ISerLogic
    {
        private const string Tag = "Ser Logic";
        
        private readonly SerModel _model;

        public SerLogic(SerModel model)
        {
            _model = model;
        }

        public async ValueTask<byte[]> Serialize<T>(string configKey, T obj)
        {
            var configs = _model.Records;
            var key = typeof(T).FullName + ":" +  configKey;
            if (!configs.TryGetValue(key, out var config))
            {
                throw new InvalidOperationException($"[{Tag}]: Config:{configKey} does not exist.");
            }

            if (config.SerSerializer is not ISerSerializer<T> ser)
            {
                throw new InvalidOperationException($"[{Tag}]: The type of the configured ISerializer does not match.");
            }
            
            var processedData = await ser.Serialize(obj);

            var processors = config.DataProcessors;
            for (var i = 0; i < processors.Count; i++)
            {
                var processor = processors[i];
                processedData = await processor.Process(processedData);
            }
            
            return processedData;
        }

        public async ValueTask<T> Deserialize<T>(string configKey,  byte[] data)
        {
            var configs = _model.Records;
            var key = typeof(T).FullName + ":" +  configKey;
            if (!configs.TryGetValue(key, out var config))
            {
                throw new InvalidOperationException($"[{Tag}]: Config:{configKey} does not exist.");
            }
            
            if (config.SerSerializer is not ISerSerializer<T> ser)
            {
                throw new InvalidOperationException($"[{Tag}]: The type of the configured ISerializer does not match.");
            }

            var processedData = data;
            var processors = config.DataProcessors;
            for (var i = processors.Count -1; i >= 0; i--)
            {
                var processor = processors[i];
                processedData = await processor.ReverseProcess(processedData);
            }
            
            var obj = await ser.Deserialize(processedData);
            return obj;
        }
    }
}