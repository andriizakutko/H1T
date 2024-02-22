using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Cache;

public class CacheProvider : ICacheProvider
{
    private readonly ILogger<CacheProvider> _logger;
    private IDatabase _db;

    public CacheProvider(IConfiguration configuration, ILogger<CacheProvider> logger)
    {
        _logger = logger;
        
        ConfigureCache(configuration);
    }

    private void ConfigureCache(IConfiguration configuration)
    {
        try
        {
            var cacheOptions = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis")!);
            var redisConnection = ConnectionMultiplexer.Connect(cacheOptions);
            _db = redisConnection.GetDatabase();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public void Init()
    {
        try
        {
            InitCategoryTypes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    private void InitCategoryTypes()
    {
        Set(CacheKeys.ProductCategoryTypes, CacheData.GetCategoryTypes());
    }

    public T Get<T>(string key)
    {
        try
        {
            var jsonValue = _db.StringGet(key);
            return jsonValue.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(jsonValue);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void Set<T>(string key, T value)
    {
        try
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            _db.StringSet(key, jsonValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}