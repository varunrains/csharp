using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace SharepointToAzureCore
{
    public static class MemoryCacheContext
    {
       static IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
       
        public static void PutValue<T>(string key, T value)
        {
            TimeSpan expirationMinutes = System.TimeSpan.FromMinutes(100);
            var expirationTime = DateTime.Now.Add(expirationMinutes);
           
            // Create cache item which executes call back function
            var cacheEntryOptions = new MemoryCacheEntryOptions()
           // Pin to cache.
           .SetPriority(Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal)
           // Set the actual expiration time
           .SetAbsoluteExpiration(expirationTime);

            cache.Set(key, value, cacheEntryOptions);
        }

        public static T GetValue<T>(string key) 
        {
            return (T)cache.Get(key);
        }

    }
}
