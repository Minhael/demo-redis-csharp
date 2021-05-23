using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace RedisPerformanceTest
{
    class Program
    {
        //  Parameters
        private const int factor = 5;
        private const string connString = "127.0.0.1:6379";
        private const int sizeGet = factor * 60;
        private const int sizeScan = factor * 10;
        private const long durationMs = 30 * 1000;

        static void Main(string[] args)
        {
            //  Configure log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("etc/log4net.config.xml"));

            //  Ops vs different volume of keys
            // logger.Info("Redis Ops:\n" + new StackExOpsTest(StackEx.Connect(connString)).Execute());

            //  Pressure test using GET
            runCachePressureTest($"StackExchange", () => StackEx.Connect(connString));
            runCachePressureTest($"NewLife", () => NewLife.Connect(connString));
            runCachePressureTest($"FreeRedis", () => FreeRedis.Connect(connString));

            //  Pressure test using SCAN
            using (var set = StackEx.Connect(connString))
            {
                runSetPressureTest($"StackExchange", set);
            }
            using (var set = NewLife.Connect(connString))
            {
                runSetPressureTest($"NewLife", set);
            }
            using (var set = FreeRedis.Connect(connString))
            {
                runSetPressureTest($"FreeRedis", set);
            }
        }

        private static void runCachePressureTest(string name, Func<Cache> factory)
        {
            try
            {
                using (var cache = factory())
                {
                    CachePressureTest.Prepare(cache);
                    var (result, elapsed) = Misc.measure(() => new CachePressureTest(sizeGet, cache, durationMs).Execute());
                    logger.Info($"{name} GET {sizeGet}: {elapsed} ms = {result}");
                }
            }
            catch (Exception e)
            {
                logger.Error($"{name} GET {sizeGet}: Failed\n{e}");
            }
        }

        private static void runSetPressureTest(string name, Set set)
        {
            try
            {
                SetPressureTest.Prepare(set);
                var (result, elapsed) = Misc.measure(() => new SetPressureTest(sizeScan, set, durationMs).Execute());
                logger.Info($"{name} SCAN {sizeScan}: {elapsed} ms = {result}");
            }
            catch (Exception e)
            {
                logger.Error($"{name} SCAN {sizeScan}: Failed\n{e}");
            }
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
    }
}
