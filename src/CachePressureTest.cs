using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using log4net;

namespace benchmark_redis_scan
{
    class CachePressureTest : TestSuite
    {
        private Func<Cache> cache;
        private int parallel;

        public CachePressureTest(int parallel, Func<Cache> cache)
        {
            this.cache = cache;
            this.parallel = parallel;
        }

        public string Execute()
        {
            //  Cache being test
            if (!cache().SetValue(KEY, "VALUE"))
                throw new InvalidOperationException("Fail to create cache");

            //  Run it
            try
            {
                var result = Observable
                .Range(0, parallel)
                .SelectMany(v =>
                    Observable.Using(() => cache(),
                        c => Generate(60 * 1000, 500, 500)
                        .SelectMany(v => GetValue(c, KEY, $"t:{v}"))
                        .SubscribeOn(NewThreadScheduler.Default)
                    )
                )
                .ObserveOn(CurrentThreadScheduler.Instance)
                .LastAsync().GetAwaiter().GetResult();

                return "Passed";
            }
            catch (Exception e)
            {
                return $"Failed with\n{e}";
            }
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

        private const string KEY = "CachePressure";
        private static readonly ILog logger = LogManager.GetLogger(typeof(CachePressureTest));
    }
}