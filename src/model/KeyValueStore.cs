namespace benchmark_redis_scan
{
    public interface KeyValueStore {
        bool SetValue(string key, string value);
        string GetValue(string key);
    }
}