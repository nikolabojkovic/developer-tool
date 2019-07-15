using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICacheProvider<T>
    {
        Task<T> GetAsync(string key);
        Task SetAsync(string key, T value, TimeSpan cacheTime);
    }
}