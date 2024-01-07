using System.Text.Json;
namespace Invoicer.Web.Extensions;

public static class ObjectExtensions
{
    public static string ToJson<T>(this T obj)
    {
        return JsonSerializer.Serialize<T>(obj);
    }

    public static bool HasDuplicates<T>(this List<T> list)
    {
        var hashset = new HashSet<T>();
        return list.Any(e => !hashset.Add(e));
    }
}
