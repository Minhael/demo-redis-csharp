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

# Benchmark

```
2021-05-23 15:43:38,749  INFO [1] - StackExchange GET 60: 30562 ms = 3514
2021-05-23 15:44:09,374  INFO [1] - NewLife GET 60: 30530 ms = 3525
2021-05-23 15:44:39,988  INFO [1] - FreeRedis GET 60: 30536 ms = 3457
2021-05-23 15:45:11,252  INFO [1] - StackExchange SCAN 10: 31251 ms = 186
2021-05-23 15:45:41,904  INFO [1] - NewLife SCAN 10: 30647 ms = 411
2021-05-23 15:46:12,949  INFO [1] - FreeRedis SCAN 10: 31031 ms = 271

2021-05-23 15:47:03,322  INFO [1] - StackExchange GET 120: 30579 ms = 6872
2021-05-23 15:47:33,970  INFO [1] - NewLife GET 120: 30550 ms = 7041
2021-05-23 15:48:04,619  INFO [1] - FreeRedis GET 120: 30572 ms = 7046
2021-05-23 15:48:36,540  INFO [1] - StackExchange SCAN 20: 31905 ms = 280
2021-05-23 15:49:07,315  INFO [1] - NewLife SCAN 20: 30770 ms = 778
2021-05-23 15:49:38,469  INFO [1] - FreeRedis SCAN 20: 31132 ms = 410

2021-05-23 15:51:11,404  INFO [1] - StackExchange GET 180: 30602 ms = 10441
2021-05-23 15:51:42,043  INFO [1] - NewLife GET 180: 30550 ms = 10457
2021-05-23 15:52:12,712  INFO [1] - FreeRedis GET 180: 30583 ms = 10560
2021-05-23 15:52:44,793  INFO [1] - StackExchange SCAN 30: 32064 ms = 325
2021-05-23 15:53:15,541  INFO [1] - NewLife SCAN 30: 30744 ms = 1120
2021-05-23 15:53:46,813  INFO [1] - FreeRedis SCAN 30: 31241 ms = 424

2021-05-23 15:54:51,496  INFO [1] - StackExchange GET 240: 30632 ms = 13819
2021-05-23 15:55:22,214  INFO [1] - NewLife GET 240: 30627 ms = 14077
2021-05-23 15:55:52,892  INFO [1] - FreeRedis GET 240: 30590 ms = 14081
2021-05-23 15:56:26,449  INFO [1] - StackExchange SCAN 40: 33536 ms = 319
2021-05-23 15:56:57,182  INFO [1] - NewLife SCAN 40: 30728 ms = 1327
2021-05-23 15:57:29,029  INFO [1] - FreeRedis SCAN 40: 31811 ms = 469

2021-05-23 15:58:18,885  INFO [1] - StackExchange GET 300: 30621 ms = 17506
2021-05-23 15:58:49,575  INFO [1] - NewLife GET 300: 30599 ms = 17541
2021-05-23 15:59:20,257  INFO [1] - FreeRedis GET 300: 30592 ms = 17624
2021-05-23 15:59:52,963  INFO [1] - StackExchange SCAN 50: 32693 ms = 261
2021-05-23 16:00:23,801  INFO [1] - NewLife SCAN 50: 30833 ms = 1651
2021-05-23 16:00:55,847  INFO [1] - FreeRedis SCAN 50: 31996 ms = 486

2021-05-23 16:05:04,196  INFO [1] - StackExchange GET 360: 30671 ms = 20744
2021-05-23 16:05:34,880  INFO [1] - NewLife GET 360: 30593 ms = 21063
2021-05-23 16:06:05,595  INFO [1] - FreeRedis GET 360: 30619 ms = 21004
2021-05-23 16:06:18,803 ERROR [1] - StackExchange SCAN 60: Failed
System.Exception: Worker exception
 ---> StackExchange.Redis.RedisTimeoutException: Timeout performing SCAN (5000ms), inst: 999, qu: 0, qs: 0, aw: False, rs: ReadAsync, ws: Idle, in: 0, in-pipe: 0, out-pipe: 0, serverEndpoint: 127.0.0.1:6379, mc: 1/1/0, mgr: 10 of 10 available, clientName: DESKTOP-P5PTF31, IOCP: (Busy=0,Free=1000,Min=32,Max=1000), WORKER: (Busy=43,Free=32724,Min=32,Max=32767), v: 2.2.4.27433 (Please take a look at this article for some common client-side issues that can cause timeouts: https://stackexchange.github.io/StackExchange.Redis/Timeouts)
   at StackExchange.Redis.CursorEnumerable`1.Enumerator.ThrowTimeout(Message message) in /_/src/StackExchange.Redis/CursorEnumerable.cs:line 244
   at StackExchange.Redis.CursorEnumerable`1.Enumerator.SlowNextSync() in /_/src/StackExchange.Redis/CursorEnumerable.cs:line 191
   at System.Linq.Enumerable.SelectEnumerableIterator`2.GetCount(Boolean onlyIfCheap)
   at System.Linq.Enumerable.Count[TSource](IEnumerable`1 source)
   at benchmark_redis_scan.SetPressureTest.<>c__DisplayClass8_0.<Execute>b__1(Int64 x) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\SetPressureTest.cs:line 96
   at benchmark_redis_scan.SetPressureTest.Generate(CancellationToken token, Int64 durationMs, Int32 periodMs, Int32 flexMs, Func`2 exe) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\SetPressureTest.cs:line 136
   at benchmark_redis_scan.SetPressureTest.<>c__DisplayClass8_0.<<Execute>b__0>d.MoveNext() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\SetPressureTest.cs:line 90
   --- End of inner exception stack trace ---
   at benchmark_redis_scan.SetPressureTest.Execute() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\SetPressureTest.cs:line 78
   at benchmark_redis_scan.Program.<>c__DisplayClass7_0.<runSetPressureTest>b__0() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 69
   at benchmark_redis_scan.Misc.measure[T](Func`1 action) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Misc.cs:line 10
   at benchmark_redis_scan.Program.runSetPressureTest(String name, Set set) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 69
2021-05-23 16:06:49,540  INFO [1] - NewLife SCAN 60: 30731 ms = 1820
2021-05-23 16:07:21,358  INFO [1] - FreeRedis SCAN 60: 31771 ms = 510
```

## 1.2.6

```
2021-05-23 16:08:39,094  INFO [1] - StackExchange GET 60: 30577 ms = 3345
2021-05-23 16:09:09,893  INFO [1] - StackExchange SCAN 10: 30769 ms = 270

2021-05-23 16:09:46,676 ERROR [1] - StackExchange GET 120: Failed
StackExchange.Redis.RedisTimeoutException: Timeout performing GET CachePressure:0:0:0:0:0:0, inst: 106, queue: 13, qu: 0, qs: 13, qc: 0, wr: 0, wq: 0, in: 11, ar: 0, clientName: DESKTOP-P5PTF31, serverEndpoint: 127.0.0.1:6379, keyHashSlot: 15911 (Please take a look at this article for some common client-side issues that can cause timeouts: http://stackexchange.github.io/StackExchange.Redis/Timeouts)
   at benchmark_redis_scan.CachePressureTest.Execute() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\CachePressureTest.cs:line 76
   at benchmark_redis_scan.Program.<>c__DisplayClass6_0.<runCachePressureTest>b__0() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 54
   at benchmark_redis_scan.Misc.measure[T](Func`1 action) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Misc.cs:line 10
   at benchmark_redis_scan.Program.runCachePressureTest(String name, Func`1 factory) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 54
2021-05-23 16:10:17,808  INFO [1] - StackExchange SCAN 20: 31109 ms = 433

2021-05-23 16:10:46,628 ERROR [1] - StackExchange GET 180: Failed
StackExchange.Redis.RedisTimeoutException: Timeout performing GET CachePressure:0:0:0:0:0:0, inst: 171, queue: 14, qu: 0, qs: 14, qc: 0, wr: 0, wq: 0, in: 22, ar: 0, clientName: DESKTOP-P5PTF31, serverEndpoint: 127.0.0.1:6379, keyHashSlot: 15911 (Please take a look at this article for some common client-side issues that can cause timeouts: http://stackexchange.github.io/StackExchange.Redis/Timeouts)
   at benchmark_redis_scan.CachePressureTest.Execute() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\CachePressureTest.cs:line 76
   at benchmark_redis_scan.Program.<>c__DisplayClass6_0.<runCachePressureTest>b__0() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 54
   at benchmark_redis_scan.Misc.measure[T](Func`1 action) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Misc.cs:line 10
   at benchmark_redis_scan.Program.runCachePressureTest(String name, Func`1 factory) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 54
2021-05-23 16:11:17,807  INFO [1] - StackExchange SCAN 30: 31156 ms = 504

2021-05-23 16:11:55,463 ERROR [1] - StackExchange GET 240: Failed
StackExchange.Redis.RedisTimeoutException: Timeout performing GET CachePressure:0:0:0:0:0:0, inst: 113, queue: 14, qu: 0, qs: 14, qc: 0, wr: 0, wq: 0, in: 22, ar: 0, clientName: DESKTOP-P5PTF31, serverEndpoint: 127.0.0.1:6379, keyHashSlot: 15911 (Please take a look at this article for some common client-side issues that can cause timeouts: http://stackexchange.github.io/StackExchange.Redis/Timeouts)
   at benchmark_redis_scan.CachePressureTest.Execute() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\CachePressureTest.cs:line 76
   at benchmark_redis_scan.Program.<>c__DisplayClass6_0.<runCachePressureTest>b__0() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 54
   at benchmark_redis_scan.Misc.measure[T](Func`1 action) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Misc.cs:line 10
   at benchmark_redis_scan.Program.runCachePressureTest(String name, Func`1 factory) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 54
2021-05-23 16:12:00,648 ERROR [1] - StackExchange SCAN 40: Failed
System.Exception: Worker exception
 ---> System.TimeoutException: The operation has timed out.
   at StackExchange.Redis.ConnectionMultiplexer.Wait[T](Task`1 task) in c:\code\StackExchange.Redis\StackExchange.Redis\StackExchange\Redis\ConnectionMultiplexer.cs:line 563
   at StackExchange.Redis.RedisBase.Wait[T](Task`1 task) in c:\code\StackExchange.Redis\StackExchange.Redis\StackExchange\Redis\RedisBase.cs:line 62
   at StackExchange.Redis.RedisBase.CursorEnumerable`1.Wait(Task`1 pending) in c:\code\StackExchange.Redis\StackExchange.Redis\StackExchange\Redis\RedisBase.cs:line 213
   at StackExchange.Redis.RedisBase.CursorEnumerable`1.CursorEnumerator.MoveNext() in c:\code\StackExchange.Redis\StackExchange.Redis\StackExchange\Redis\RedisBase.cs:line 281
   at System.Linq.Enumerable.SelectEnumerableIterator`2.GetCount(Boolean onlyIfCheap)
   at System.Linq.Enumerable.Count[TSource](IEnumerable`1 source)
   at benchmark_redis_scan.SetPressureTest.<>c__DisplayClass8_0.<Execute>b__1(Int64 x) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\SetPressureTest.cs:line 96
   at benchmark_redis_scan.SetPressureTest.Generate(CancellationToken token, Int64 durationMs, Int32 periodMs, Int32 flexMs, Func`2 exe) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\SetPressureTest.cs:line 136
   at benchmark_redis_scan.SetPressureTest.<>c__DisplayClass8_0.<<Execute>b__0>d.MoveNext() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\SetPressureTest.cs:line 90
   --- End of inner exception stack trace ---
   at benchmark_redis_scan.SetPressureTest.Execute() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\suite\SetPressureTest.cs:line 78
   at benchmark_redis_scan.Program.<>c__DisplayClass7_0.<runSetPressureTest>b__0() in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 69
   at benchmark_redis_scan.Misc.measure[T](Func`1 action) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Misc.cs:line 10
   at benchmark_redis_scan.Program.runSetPressureTest(String name, Set set) in C:\Users\User\workspace\csharp\benchmark-redis-csharp\src\Program.cs:line 69
```