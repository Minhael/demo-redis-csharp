using System;

namespace benchmark_redis_scan
{
    public static class Misc {
        public static (T, long) measure<T>(Func<T> action) {
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var rt = action();
            return (rt, DateTimeOffset.Now.ToUnixTimeMilliseconds() - timestamp);
        }

        public static long measure(Action action) {
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            action();
            return DateTimeOffset.Now.ToUnixTimeMilliseconds() - timestamp;
        }

        public static Func<string, string> Pad(int width) {
            return s => String.Format($"{{0, -{width}}}", s);
        }

        public static string Pipe(string x, string y) {
            return x + " | " + y;
        }
    }
}