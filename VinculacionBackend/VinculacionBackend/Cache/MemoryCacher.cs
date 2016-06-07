using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Cache
{
    public class MemoryCacher:IMemoryCacher
    {
        private object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }

        private bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }

        private void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
         public IEnumerable<Major> GetMajors(IMajorsServices majorsServices)
        {
            var majors = GetValue("Majors");
            if (majors == null)
            {
                Add("Majors", majorsServices.All().ToList(), DateTimeOffset.UtcNow.AddHours(24));
                majors =GetValue("Majors");
            }
            return majors as IEnumerable<Major>;
        }
    }
}