using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace benchmark_redis_scan
{
    class Redis : Cache, Set {
        public static Redis Connect(string host, int port) {
            
            //  Make Connection
            var conn = ConnectionMultiplexer.Connect($"{host}:{port}");

            //  Get the exact Redis server
            var server = conn.GetServer("localhost", 6379);
            
            return new Redis(conn, server);
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

        public void Close() {
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

        private IDatabase Api() {
            return conn.GetDatabase();
        }
    }
}