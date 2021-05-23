using System;
using System.Numerics;
using log4net;

namespace benchmark_redis_scan
{
    public static class Misc {
        public static (T, long) measure<T>(Func<T> action) {
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var rt = action();
            return (rt, DateTimeOffset.Now.ToUnixTimeMilliseconds() - timestamp);
        }

        public static long measure(Action action) {
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            action();
            return DateTimeOffset.Now.ToUnixTimeMilliseconds() - timestamp;
        }

        public static Func<string, string> Pad(int width) {
            return s => String.Format($"{{0, -{width}}}", s);
        }

        public static string Pipe(string x, string y) {
            return x + " | " + y;
        }

        /**
         * Generate n cache in m layers, for n < population and m = vertical
        **/
        public static long GeneratePopulation(KeyValueStore kvs, int population = 256, int vertical = 6, string prefix = "", string content = "") {
            var horizontal = Convert.ToInt32(Math.Floor(Math.Exp(Math.Log(population) / (vertical + 1))));
            var count = (long) BigInteger.Pow(horizontal, vertical + 1);

            logger.Info($"Generate h: {horizontal} v: {vertical} c: {count} for p: {population}");
            
            var timeElasped = Misc.measure(() => _GeneratePopulation(kvs, horizontal, vertical, content, prefix));
            logger.Info($"Cache set in {timeElasped} ms");
            
            return count;
        }

        private static void _GeneratePopulation(KeyValueStore kvs, int horizontal = 7, int vertical = 6, string content = "", string key = "", long value = 0, int depth = 0) {
            if (depth < vertical - 1) {
                for (int i = 0; i < horizontal; ++i) {
                    _GeneratePopulation(kvs, horizontal, vertical, content, key + $":{i}", value + i * (long)BigInteger.Pow(horizontal, vertical - depth), depth + 1);
                }
            } else {
                for (int i = 0; i < horizontal; ++i) {
                    if (!kvs.SetValue(key + $":{i}", $"{value + i}={content}"))
                        throw new InvalidOperationException($"Fail to Set {key}:{i}={value + i}");
                }
            }
        }
        private static readonly ILog logger = LogManager.GetLogger(typeof(Misc));
    }
}