using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace RedisPerformanceTest
{
    class StackExOpsTest : TestSuite
    {
        public static StackExOpsTest WithContent(StackEx redis, string filePath = "etc/321gone.xml") {
            //  Read additional data for filling up value
            return new StackExOpsTest(redis, File.ReadAllText(filePath));
        }

        private StackEx redis;
        private String content;

        public StackExOpsTest(StackEx redis, string content = "") {
            this.redis = redis;
            this.content = content;
        }

        public string Execute()
        {
            var (r128, c128) = Run4Population(redis, 128, content);
            var (r65536, c65536) = Run4Population(redis, 65536, content);
            var (r262144, c262144) = Run4Population(redis, 262144, content);
            var (r524288, c524288) = Run4Population(redis, 524288, content);
            var (r1048576, c1048576) = Run4Population(redis, 1048576, content);

            //  Build reports
            var report = String.Join("\n",
                new List<string>() { "" }.Concat(r128.Keys).Select(Misc.Pad(15))
                    .Zip(new List<string>() { $"Size {c128}" }.Concat(r128.Values).Select(Misc.Pad(8)), Misc.Pipe)
                    .Zip(new List<string>() { $"Size {c65536}" }.Concat(r65536.Values).Select(Misc.Pad(8)), Misc.Pipe)
                    .Zip(new List<string>() { $"Size {c262144}" }.Concat(r262144.Values).Select(Misc.Pad(8)), Misc.Pipe)
                    .Zip(new List<string>() { $"Size {c524288}" }.Concat(r524288.Values).Select(Misc.Pad(8)), Misc.Pipe)
                    .Zip(new List<string>() { $"Size {c1048576}" }.Concat(r1048576.Values).Select(Misc.Pad(8)), Misc.Pipe)
            );

            return report;
        }

        private static (IDictionary<string, string>, long) Run4Population(StackEx redis, int population, string content = "") {
            //  Benchmark is not gonna working when population < 128
            population = Math.Max(128, population);

            //  Fixed vertical depth at default = 6
            var size = Misc.GeneratePopulation(redis, population, 6, content, $"{PREFIX}");

            //  Execute the test
            return (Benchmark(redis), size);
        }

        /**
         * Assumed at least 2 vertical
        **/
        private static IDictionary<string, string> Benchmark(StackEx redis) {
            var rt = new SortedDictionary<string, string>();

            // Single operation
            var key = "test:single:6:layer:cache:key";
            rt.Add("01-Set", Misc.measure(() => redis.SetValue(key, "E")).Item2.ToString());
            rt.Add("02-Update", Misc.measure(() => redis.SetValue(key, "A")).Item2.ToString());
            rt.Add("03-Get", Misc.measure(() => redis.GetValue(key) == "A").Item2.ToString());
            rt.Add("04-Remove", Misc.measure(() => redis.Remove(key)).Item2.ToString());

            //  Searches
            var (k33, timeK33) = Misc.measure(() => redis.KeySetRaw($"{PREFIX}:0:0:*", 250));
            rt.Add("05-250 Raw", timeK33.ToString());

            var (k23, timeK23) = Misc.measure(() => redis.KeySet($"{PREFIX}:0:1:*", 250));
            rt.Add("06-250", timeK23.ToString());

            rt.Add("07-10", Misc.measure(() => redis.KeySet($"{PREFIX}:1:0:*", 10)).Item2.ToString());

            var (full, timeElasped) = Misc.measure(() => redis.KeySet($"{PREFIX}:*", 10000));
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

        private const string PREFIX = "RedisOps";
    }
}