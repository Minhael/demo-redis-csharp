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
        private const int clientNumber = 70;

        static void Main(string[] args)
        {
            //  Configure log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("etc/log4net.config.xml"));

            //  Ops vs different volume of keys
            // logger.Info("Redis Ops:\n" + new RedisOpsTest(Redis.Connect(hostname, port)).Execute());

            //  Pressure test with multiple clients
            var (result, elapsed) = Misc.measure(() => new CachePressureTest(clientNumber, () => Redis.Connect(connString)).Execute());
            logger.Info($"Pressure Test completed in {elapsed} ms: {result}");
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
    }
}
