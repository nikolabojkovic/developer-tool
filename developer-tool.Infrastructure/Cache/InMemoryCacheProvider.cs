using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Infrastructure.Caching
{
    public class InMemoryCacheProvider : ICacheProvider
    {

        private readonly IDistributedCache _cache;
        public CacheOptions CacheOptions { get; }

        public InMemoryCacheProvider(IDistributedCache cache, IOptions<CacheOptions> cacheOptions)
        {
            _cache = cache;
            CacheOptions = cacheOptions.Value;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            if (!CacheOptions.IsCachingEnabled)
                return default(T);

            var arrBytes = await _cache.GetAsync(key);
            if (arrBytes == null)
                return default(T);

            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj as T;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan cacheTime) where T : class
        {
            if (value == null)
                return;

            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, value);
                await _cache.SetAsync(key, 
                                      ms.ToArray(),
                                      new DistributedCacheEntryOptions().SetAbsoluteExpiration(cacheTime));
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}