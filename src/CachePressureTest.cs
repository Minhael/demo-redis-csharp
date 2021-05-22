using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using log4net;

namespace benchmark_redis_scan
{
    class CachePressureTest : TestSuite
    {
        private Cache cache;
        private int parallel;

        public CachePressureTest(int parallel, Cache cache)
        {
            this.cache = cache;
            this.parallel = parallel;
        }

        public string Execute()
        {
            //  Cache being test
            if (!cache.SetValue(KEY_CACHE_PRESSURE, VALUE_CACHE_PRESSURE))
                throw new InvalidOperationException("Fail to create cache");

            //  Run it
            var result = Observable
            .Range(0, parallel)
            .SelectMany(v =>
                Generate(60 * 1000, 500, 500)
                    .SelectMany(v => GetValue(cache, KEY_CACHE_PRESSURE, $"t:{v}"))
                    .Where(v => v == VALUE_CACHE_PRESSURE)
                    .Count()
                    .SubscribeOn(NewThreadScheduler.Default)
            )
            .Aggregate(0, (a, v) => a + v)
            .ObserveOn(CurrentThreadScheduler.Instance)
            .LastAsync().GetAwaiter().GetResult();

            return result.ToString();
        }

        private static IObservable<long> Generate(long durationMs, long periodMs, int flexMs)
        {
            var r = new Random();
            return Observable
            .Interval(TimeSpan.FromMilliseconds(periodMs))
            .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(durationMs)))
            .SelectMany(v =>
            {
                var delayMs = r.Next(flexMs);
                return Observable
                .Return(v + delayMs)
                .Delay(TimeSpan.FromMilliseconds(delayMs));
            });
        }

        private static IObservable<string> GetValue(Cache cache, string key, string tag = "")
        {
            return Observable.Start(() =>
            {
                logger.Info($"[{tag}]: GET");
                return cache.GetValue(key);
            });
        }

        private const string KEY_CACHE_PRESSURE = "CachePressure";
        private const string VALUE_CACHE_PRESSURE = "VALUE";
        private static readonly ILog logger = LogManager.GetLogger(typeof(CachePressureTest));
    }
}