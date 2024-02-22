namespace Infrastructure.Cache;

public interface ICacheProvider
{
    void Init();
    T Get<T>(string key);
    void Set<T>(string key, T value);
}