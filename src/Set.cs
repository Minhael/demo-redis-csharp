using System.Collections.Generic;

namespace benchmark_redis_scan
{
    interface Set {
        IEnumerable<string> KeySet(string pattern = null, int pageSize = 250);
        bool Remove(string key);
        long Remove(params string[] key);
        long Size();
        void Clear();
    }
}