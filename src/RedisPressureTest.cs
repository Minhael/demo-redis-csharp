using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using log4net;

namespace benchmark_redis_scan
{
    /**
     * https://deniskyashif.com/2020/01/07/csharp-channels-part-3/
    **/
    class SetPressureTest : TestSuite
    {
        private Set cache;
        private int parallel;

        public SetPressureTest(int parallel, Set cache)
        {
            this.cache = cache;
            this.parallel = parallel;
        }

        public string Execute()
        {
            //  Cache being test
            if (!cache.SetValue(KEY_CACHE_PRESSURE, VALUE_CACHE_PRESSURE))
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
                    await foreach (var (count, e) in Execute(token, cache, clientNumber).Reader.ReadAllAsync())
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

        private static Channel<(long, Exception)> Execute(CancellationToken token, Set cache, int clientNumber)
        {
            var ec = Channel.CreateUnbounded<(long, Exception)>();

            Task.Run(async () =>
            {
                try
                {
                    var result = await Generate(token, 60 * 1000, 500, 500, x =>
                    {
                        logger.Info($"[t:{clientNumber}]: SCAN");
                        
                        var value = cache.KeySet($"{KEY_CACHE_PRESSURE}");
                        
                        if (value.Count() != 1)
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