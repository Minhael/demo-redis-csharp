using System;
using System.Numerics;
using log4net;

namespace RedisPerformanceTest
{
    interface Cache: KeyValueStore, IDisposable {
    }
}