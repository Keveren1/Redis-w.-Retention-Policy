using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Redis_w._Retention_Policy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly IDatabase _redisDb;

        public CacheController()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            _redisDb = redis.GetDatabase();
        }

        [HttpPost]
        public IActionResult SetValue([FromQuery] string key, [FromQuery] string value)
        {
            _redisDb.StringSet(key, value, TimeSpan.FromDays(1)); // Change FromDays(1) to FromSeconds(10) to quickly confirm that it auto deletes 
            return Ok("Key saved with TTL");
        }

        [HttpGet]
        public IActionResult GetValue([FromQuery] string key)
        {
            var value = _redisDb.StringGet(key);
            return value.HasValue ? Ok(value.ToString()) : NotFound();
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] string key, [FromQuery] string value)
        {
            bool updated = _redisDb.StringSet(key, value, when: When.Exists); // Update only if exists
            return updated ? Ok() : NotFound();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string key)
        {
            bool deleted = _redisDb.KeyDelete(key); // Delete
            return deleted ? Ok() : NotFound();
        }
    }
}
