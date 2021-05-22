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
        private const string connString = "localhost:6379";
        private const int parallelism = 200;

        static void Main(string[] args)
        {
            //  Configure log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("etc/log4net.config.xml"));

            //  Ops vs different volume of keys
            // logger.Info("Redis Ops:\n" + new RedisOpsTest(Redis.Connect(hostname, port)).Execute());

            //  Pressure test with multiple clients
            try
            {
                using (var cache = Redis.Connect(connString))
                {
                    var (result, elapsed) = Misc.measure(() => new CachePressureTest(parallelism, cache).Execute());
                    logger.Info($"Completed in {elapsed} ms size {result}");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Failed\n{e}");
            }
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
    }
}
