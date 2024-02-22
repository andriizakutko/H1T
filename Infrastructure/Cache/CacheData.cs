using Domain;
using Newtonsoft.Json;

namespace Infrastructure.Cache;

public static class CacheData
{
    public static ICollection<CategoryTypes> GetCategoryTypes()
    {
        var jsonContent = File.ReadAllText(Directory.GetCurrentDirectory() + "/static/categoryTypes.json");

        return JsonConvert.DeserializeObject<List<CategoryTypes>>(jsonContent);
    }
}