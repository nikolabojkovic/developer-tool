using System;
using System.Threading.Tasks;
using Core.Options;

namespace Core.Interfaces
{
    public interface ICacheProvider
    {
        CacheOptions CacheOptions { get; }
        
        Task<T> GetAsync<T>(string key) where T : class;
        Task SetAsync<T>(string key, T value, TimeSpan cacheTime) where T : class;
        Task RemoveAsync(string key);
    }
}