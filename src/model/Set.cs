using System;
using System.Numerics;
using System.Collections.Generic;
using log4net;

namespace benchmark_redis_scan
{
    interface Set: KeyValueStore {
        IEnumerable<string> KeySet(string pattern = null, int pageSize = 250);
        bool Remove(string key);
        long Remove(params string[] key);
        long Size();
        void Clear();
    }
}