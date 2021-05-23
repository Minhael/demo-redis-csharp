using System;
using System.Collections.Generic;
using System.Linq;
using FreeRedis;
using log4net;

namespace benchmark_redis_scan
{
    class FreeRedis : Cache, Set {

        public static FreeRedis Connect(string connString) {
            return new FreeRedis(new RedisClient(connString));
        }

        private RedisClient redis;

        public FreeRedis(RedisClient redis) {
            this.redis = redis;
        }

        public void Clear()
        {
            redis.FlushAll();
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
            return redis.Scan(pattern, pageSize, null).SelectMany(x => x.ToList());
        }

        public bool Remove(string key)
        {
            return redis.Del(key) > 0;
        }

        public long Remove(params string[] key)
        {
            return redis.Del(key);
        }

        public bool SetValue(string key, string value)
        {
            redis.Set(key, value);
            return true;
        }

        public long Size()
        {
            return redis.DbSize();
        }
        
        private static readonly ILog logger = LogManager.GetLogger(typeof(NewLife));
    }
}