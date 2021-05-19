using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;
using log4net.Config;

namespace benchmark_redis_scan
{
    class Program
    {
        //  Parameters
        private const string hostname = "localhost";
        private const int port = 6379;

        static void Main(string[] args)
        {
            //  Configure log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("etc/log4net.config.xml"));

            //  Connect to redis
            logger.Info($"Connect to {hostname}:{port}");
            var redis = Redis.Connect(hostname, port);

            VolumeTest(redis);
        }

        private static void VolumeTest(Redis redis) {
            //  Read additional data for filling up value
            // string content = File.ReadAllText("etc/321gone.xml");
            string content = "";

            //  Benchmarks
            var (r128, c128) = Run4Population(redis, 128, content);
            var (r65536, c65536) = Run4Population(redis, 65536, content);
            var (r262144, c262144) = Run4Population(redis, 262144, content);
            var (r524288, c524288) = Run4Population(redis, 524288, content);
            var (r1048576, c1048576) = Run4Population(redis, 1048576, content);

            //  Build reports
            var report = String.Join("\n",
                new List<string>() { "" }.Concat(r128.Keys).Select(Pad(15))
                    .Zip(new List<string>() { $"Size {c128}" }.Concat(r128.Values).Select(Pad(8)), Pipe)
                    .Zip(new List<string>() { $"Size {c65536}" }.Concat(r65536.Values).Select(Pad(8)), Pipe)
                    .Zip(new List<string>() { $"Size {c262144}" }.Concat(r262144.Values).Select(Pad(8)), Pipe)
                    .Zip(new List<string>() { $"Size {c524288}" }.Concat(r524288.Values).Select(Pad(8)), Pipe)
                    .Zip(new List<string>() { $"Size {c1048576}" }.Concat(r1048576.Values).Select(Pad(8)), Pipe)
            );

            //  Print result
            logger.Info("\n" + report);
        }
        private static Func<string, string> Pad(int width) {
            return s => String.Format($"{{0, -{width}}}", s);
        }

        private static string Pipe(string x, string y) {
            return x + " | " + y;
        }

        private static (IDictionary<string, string>, long) Run4Population(Redis redis, int population, string content = "") {
            //  Benchmark is not gonna working when population < 128
            population = Math.Max(128, population);

            //  Fixed vertical depth at default = 6
            var size = Cache.GenerateCache(redis, population, 6, content);

            //  Execute the test
            return (Benchmark(redis), size);
        }

        /**
         * Assumed at least 2 vertical
        **/
        private static IDictionary<string, string> Benchmark(Redis redis) {
            var rt = new SortedDictionary<string, string>();

            // Single operation
            var key = "test:single:6:layer:cache:key";
            rt.Add("01-Set", Misc.measure(() => redis.SetValue(key, "E")).Item2.ToString());
            rt.Add("02-Update", Misc.measure(() => redis.SetValue(key, "A")).Item2.ToString());
            rt.Add("03-Get", Misc.measure(() => redis.GetValue(key) == "A").Item2.ToString());
            rt.Add("04-Remove", Misc.measure(() => redis.Remove(key)).Item2.ToString());

            //  Searches
            var (k33, timeK33) = Misc.measure(() => redis.KeySetRaw(":0:0:*", 250));
            rt.Add("05-250 Raw", timeK33.ToString());

            var (k23, timeK23) = Misc.measure(() => redis.KeySet(":0:1:*", 250));
            rt.Add("06-250", timeK23.ToString());

            rt.Add("07-10", Misc.measure(() => redis.KeySet(":1:0:*", 10)).Item2.ToString());

            var (full, timeElasped) = Misc.measure(() => redis.KeySet("*", 10000));
            rt.Add("08-10000", timeElasped.ToString());

            //  Bulk operation
            var k33List = k33.ToList();
            rt.Add("09-Loop Remove", Misc.measure(() => k33List.ForEach(k => redis.Remove(k))).ToString());

            var k23Array = k23.ToArray();
            rt.Add("10-Bulk Remove", Misc.measure(() => redis.Remove(k23Array)).Item2.ToString());

            var fullArray = full.ToArray();
            //  Clear is only available to admin so we remove it 1 by 1
            rt.Add("11-Clean up", Misc.measure(() => redis.Remove(fullArray)).Item2.ToString());

            return rt;
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
    }
}
