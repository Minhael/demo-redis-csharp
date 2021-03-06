using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using log4net;

namespace RedisPerformanceTest
{
    /**
     * https://deniskyashif.com/2020/01/07/csharp-channels-part-3/
    **/
    class SetPressureTest : TestSuite
    {
        public static void Prepare(Set set) {
            //  Create 820k caches sample
            if (set.GetValue(KEY_CACHE_PRESSURE) == null) {
                Misc.GeneratePopulation(set, 1048576, 6, KEY_CACHE_PRESSURE, File.ReadAllText(@"etc/cd_catalog.xml"));
                set.SetValue(KEY_CACHE_PRESSURE, "1");
            }
        }

        private Set set;
        private int parallel;

        private long durationMs;
        private int periodMs;
        private int flexMs;

        public SetPressureTest(int parallel, Set set, long durationMs = 60 * 1000, int periodMs = 500, int flexMs = 500)
        {
            this.set = set;
            this.parallel = parallel;
            this.durationMs = durationMs;
            this.periodMs = periodMs;
            this.flexMs = flexMs;
        }

        public string Execute()
        {
            //  Cache being test
            if (!set.SetValue($"{KEY_CACHE_PRESSURE}:0:0:0:0:0:0", VALUE_CACHE_PRESSURE))
                throw new InvalidOperationException("Fail to create cache");

            //  Global cancel signal
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            //  Create parallel execution
            long result = 0;
            Exception err = null;
            var tasks = new List<Task>();
            for (int i = 0; i < parallel; ++i)
            {
                var clientNumber = i;
                var task = Task.Run(async () =>
                {
                    await foreach (var (count, e) in Execute(token, set, clientNumber).Reader.ReadAllAsync())
                    {
                        result += count;
                        if (e != null && e is not OperationCanceledException && !source.IsCancellationRequested)
                        {
                            source.Cancel();
                            err = e;
                        }
                    }
                });
                tasks.Add(task);
            }

            //  Wait for all worker finish
            Task.WhenAll(tasks).Wait();

            //  Rethrow exception
            if (err != null)
                throw new Exception("Worker exception", err);
            return result.ToString();
        }

        private Channel<(long, Exception)> Execute(CancellationToken token, Set cache, int clientNumber)
        {
            var ec = Channel.CreateUnbounded<(long, Exception)>();

            Task.Run(async () =>
            {
                try
                {
                    var result = await Generate(token, durationMs, periodMs, flexMs, x =>
                    {
                        logger.Debug($"[t:{clientNumber}]: SCAN");
                        
                        var value = cache.KeySet($"{KEY_CACHE_PRESSURE}:0:0:*");
                        
                        if (value.Count() < 1)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    });
                    await ec.Writer.WriteAsync((result, null));
                }
                catch (Exception e)
                {
                    await ec.Writer.WriteAsync((0, e));
                }
                finally
                {
                    ec.Writer.Complete();
                }
            });

            return ec;
        }

        private static async Task<long> Generate(CancellationToken token, long durationMs, int periodMs, int flexMs, Func<long, long> exe)
        {

            var r = new Random(Guid.NewGuid().GetHashCode());
            long count = 0;
            long elapsed = 0;

            while (elapsed < durationMs)
            {
                var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var delayMs = r.Next(flexMs);

                if (token.IsCancellationRequested)
                    return count;

                await Task.Delay(delayMs);
                count += exe(elapsed);

                if (token.IsCancellationRequested)
                    return count;

                await Task.Delay(Math.Max(0, periodMs - delayMs));
                elapsed += DateTimeOffset.Now.ToUnixTimeMilliseconds() - now;
            }

            return count;
        }

        private const string KEY_CACHE_PRESSURE = "CachePressure";
        private const string VALUE_CACHE_PRESSURE = "VALUE";
        private static readonly ILog logger = LogManager.GetLogger(typeof(CachePressureTest));
    }
}