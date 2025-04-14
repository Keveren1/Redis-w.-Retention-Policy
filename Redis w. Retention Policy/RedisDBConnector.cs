using StackExchange.Redis;

namespace Redis_w._Retention_Policy
{
    public class RedisConnection
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("localhost");
        });

        public static ConnectionMultiplexer Connection => lazyConnection.Value;
    }
}
