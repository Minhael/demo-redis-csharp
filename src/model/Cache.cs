using System;
using System.Numerics;
using log4net;

namespace benchmark_redis_scan
{
    interface Cache: KeyValueStore, IDisposable {
    }
}