using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Caching
{
    public class InMemoryCacheProvider<T> : ICacheProvider<T> where T : class
    {

        private readonly IDistributedCache _cache;

        public InMemoryCacheProvider(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync(string key)
        {
            var arrBytes = await _cache.GetAsync(key);

                using (var memStream = new MemoryStream())
                {
                    var binForm = new BinaryFormatter();
                    memStream.Write(arrBytes, 0, arrBytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    var obj = binForm.Deserialize(memStream);
                    return obj as T;
                }
        }

        public async Task SetAsync(string key, T value, TimeSpan cacheTime)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, value);
                await _cache.SetAsync(key, 
                                      ms.ToArray(),
                                      new DistributedCacheEntryOptions().SetSlidingExpiration(cacheTime));
            }
        }
    }
}