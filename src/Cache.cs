using System;
using System.Numerics;
using System.Collections.Generic;
using log4net;

namespace benchmark_redis_scan
{
    interface Cache: IDisposable {
        bool SetValue(string key, string value);
        string GetValue(string key);

        /**
         * Generate n cache in m layers, for n < population and m = vertical
        **/
        public static long GenerateCache(Cache cache, int population = 256, int vertical = 6, string prefix = "", string content = "") {
            var horizontal = Convert.ToInt32(Math.Floor(Math.Exp(Math.Log(population) / (vertical + 1))));
            var count = (long) BigInteger.Pow(horizontal, vertical + 1);

            logger.Info($"Generate h: {horizontal} v: {vertical} c: {count} for p: {population}");
            
            var timeElasped = Misc.measure(() => _GenerateCache(cache, horizontal, vertical, content, prefix));
            logger.Info($"Cache set in {timeElasped}ms");
            
            return count;
        }

        private static void _GenerateCache(Cache cache, int horizontal = 7, int vertical = 6, string content = "", string key = "", long value = 0, int depth = 0) {
            if (depth < vertical) {
                for (int i = 0; i < horizontal; ++i) {
                    _GenerateCache(cache, horizontal, vertical, content, key + $":{i}", value + i * (long)BigInteger.Pow(horizontal, vertical - depth), depth + 1);
                }
            } else {
                for (int i = 0; i < horizontal; ++i) {
                    if (!cache.SetValue(key + $":{i}", $"{value + i}={content}"))
                        throw new InvalidOperationException($"Fail to Set {key}:{i}={value + i}");
                }
            }
        }
        private static readonly ILog logger = LogManager.GetLogger(typeof(Cache));
    }
}