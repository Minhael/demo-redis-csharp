using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace benchmark_redis_scan
{
    class Program
    {
        //  Parameters
        private const string connString = "127.0.0.1:6379";
        private const int parallelism = 400;

        static void Main(string[] args)
        {
            //  Configure log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("etc/log4net.config.xml"));

            //  Ops vs different volume of keys
            // logger.Info("Redis Ops:\n" + new RedisOpsTest(Redis.Connect(hostname, port)).Execute());

            //  Pressure test with multiple clients
            // runCachePressureTest("StackExchange", () => StackEx.Connect(connString));
            // runCachePressureTest("NewLife", () => NewLife.Connect(connString));
            // runCachePressureTest("FreeRedis", () => FreeRedis.Connect(connString));

            using (var set = StackEx.Connect(connString))
            {
                runSetPressureTest("StackExchange", set);
            }
            using (var set = NewLife.Connect(connString))
            {
                runSetPressureTest("NewLife", set);
            }
            using (var set = FreeRedis.Connect(connString))
            {
                runSetPressureTest("FreeRedis", set);
            }
        }

        private static void runSetPressureTest(string name, Set set)
        {
            try
            {
                var (result, elapsed) = Misc.measure(() => new SetPressureTest(parallelism, set).Execute());
                logger.Info($"{name}: {elapsed} ms = {result}");
            }
            catch (Exception e)
            {
                logger.Error($"{name}: Failed\n{e}");
            }
        }

        private static void runCachePressureTest(string name, Func<Cache> factory)
        {
            try
            {
                using (var cache = factory())
                {
                    var (result, elapsed) = Misc.measure(() => new CachePressureTest(parallelism, cache).Execute());
                    logger.Info($"{name}: {elapsed} ms = {result}");
                }
            }
            catch (Exception e)
            {
                logger.Error($"{name}: Failed\n{e}");
            }
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
    }
}
