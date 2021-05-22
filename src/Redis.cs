using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using log4net;

namespace benchmark_redis_scan
{
    class Redis : Cache, Set, Measurable {
        public static Redis Connect(string connString) {

            logger.Debug($"Connect to {connString}");
            
            //  Make Connection
            var conn = ConnectionMultiplexer.Connect($"{connString}");

            //  Get the exact Redis server
            var server = conn.GetServer(connString);
            
            return new Redis(conn, server);
        }

        public static Redis Connect(string host, int port) {
            return Connect($"{host}:{port}");
        }

        private ConnectionMultiplexer conn { get; }
        private IServer server { get; }

        public Redis(ConnectionMultiplexer conn, IServer server) {
            this.conn = conn;
            this.server = server;
        }

        public bool SetValue(string key, string value) {
            return Api().StringSet(key, value);
        }
        public string GetValue(string key) {
            return Api().StringGet(key);
        }

        public void Dispose()
        {
            conn.Close();
        }

        public IEnumerable<RedisKey> KeySetRaw(string pattern = null, int pageSize = 250)
        {
            return server.Keys(pattern: pattern ?? "*", pageSize: pageSize);
        }

        public IEnumerable<string> KeySet(string pattern = null, int pageSize = 250)
        {
            return KeySetRaw(pattern, pageSize).Select(x => x.ToString());
        }

        public bool Remove(string key)
        {
            return Api().KeyDelete(key);
        }

        public long Remove(params string[] key)
        {
            return Api().KeyDelete(key.Select(k => (RedisKey) k).ToArray());
        }

        public long Size()
        {
            return KeySet().Count();
        }

        public void Clear()
        {
            server.FlushDatabase();
        }

        public Telemetric measure()
        {
            return new RedisTelemetic();
        }

        private IDatabase Api() {
            return conn.GetDatabase();
        }

        private class RedisTelemetic : Telemetric, IDisposable
        {

            public void SetDumpToConsole(bool isEnable)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(Redis));
    }
}