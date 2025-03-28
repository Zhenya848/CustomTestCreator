namespace CustomTestCreator.Core.Application.Extensions;

public static class QueriesExtensions
{
    public static IQueryable<T> GetItemsWithPagination<T>(
        this IQueryable<T> source,
        int page,
        int pageSize)
    {
        var items = source
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        
        return items;
    }
}