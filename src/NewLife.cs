using System;
using System.Collections.Generic;
using NewLife.Caching;
using NewLife.Caching.Models;
using log4net;

namespace RedisPerformanceTest
{
    class NewLife : Cache, Set {

        public static NewLife Connect(string connString) {
            return new NewLife(new FullRedis(connString, "", 0));
        }

        private FullRedis redis;

        public NewLife(FullRedis redis) {
            this.redis = redis;
        }

        public void Clear()
        {
            redis.Clear();
        }

        public void Dispose()
        {
            redis.Dispose();
        }

        public string GetValue(string key)
        {
            return redis.Get<string>(key);
        }

        public IEnumerable<string> KeySet(string pattern = null, int pageSize = 250)
        {
            var model = new SearchModel();
            model.Pattern = pattern;
            model.Count = pageSize;
            return redis.Search(model);
        }

        public bool Remove(string key)
        {
            return redis.Remove(key) > 0;
        }

        public long Remove(params string[] key)
        {
            return redis.Remove(key);
        }

        public bool SetValue(string key, string value)
        {
            return redis.Set(key, value);
        }

        public long Size()
        {
            return redis.Count;
        }
        private static readonly ILog logger = LogManager.GetLogger(typeof(NewLife));
    }
}