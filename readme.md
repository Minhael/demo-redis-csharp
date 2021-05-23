# demo-redis-csharp

https://github.com/StackExchange/StackExchange.Redis/issues/1334

# StackExchange.Redis Stacktrace

Reference: https://github.com/StackExchange/StackExchange.Redis/blob/main/docs/Timeouts.md
```
System.Exception: Worker exception
 ---> StackExchange.Redis.RedisTimeoutException: Timeout performing SCAN, inst: 303, queue: 14, qu: 0, qs: 14, qc: 0, wr: 0, wq: 0, in: 315, ar: 0, clientName: DESKTOP-P5PTF31, serverEndpoint: Unspecified/localhost:6379 (Please take a look at this article for some common client-side issues that can cause timeouts: http://stackexchange.github.io/StackExchange.Redis/Timeouts)
   at StackExchange.Redis.ConnectionMultiplexer.ExecuteSyncImpl[T](Message message, ResultProcessor`1 processor, ServerEndPoint server) in c:\code\StackExchange.Redis\StackExchange.Redis\StackExchange\Redis\ConnectionMultiplexer.cs:line 2120
   at StackExchange.Redis.RedisServer.ExecuteSync[T](Message message, ResultProcessor`1 processor, ServerEndPoint server) in c:\code\StackExchange.Redis\StackExchange.Redis\StackExchange\Redis\RedisServer.cs:line 569
   at StackExchange.Redis.RedisBase.CursorEnumerable`1.GetNextPageSync(IScanningCursor obj, Int64 cursor) in c:\code\StackExchange.Redis\StackExchange.Redis\StackExchange\Redis\RedisBase.cs:line 204
   at StackExchange.Redis.RedisBase.CursorEnumerable`1.CursorEnumerator.MoveNext() in c:\code\StackExchange.Redis\StackExchange.Redis\StackExchange\Redis\RedisBase.cs:line 273
   at System.Linq.Enumerable.SelectEnumerableIterator`2.GetCount(Boolean onlyIfCheap)
   at benchmark_redis_scan.RedisPressureTest.<>c__DisplayClass4_0.<Execute>b__1(Int64 x) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\RedisPressureTest.cs:line 80
   at benchmark_redis_scan.RedisPressureTest.Generate(CancellationToken token, Int64 durationMs, Int32 periodMs, Int32 flexMs, Func`2 exe) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\RedisPressureTest.cs:line 120     
   at benchmark_redis_scan.RedisPressureTest.<>c__DisplayClass4_0.<<Execute>b__0>d.MoveNext() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\RedisPressureTest.cs:line 74
   --- End of inner exception stack trace ---
   at benchmark_redis_scan.RedisPressureTest.Execute() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\RedisPressureTest.cs:line 62
   at benchmark_redis_scan.Program.<>c__DisplayClass2_0.<Main>b__0() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 29
   at benchmark_redis_scan.Misc.measure[T](Func`1 action) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Misc.cs:line 8
   at benchmark_redis_scan.Program.Main(String[] args) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 29
```