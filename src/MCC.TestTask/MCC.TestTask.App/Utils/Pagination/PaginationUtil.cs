namespace MCC.TestTask.App.Utils.Pagination;

public static class PaginationUtil
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> query, PaginationModel pagination)
    {
        return query
            .Skip((pagination.Page - 1) * pagination.Size)
            .Take(pagination.Size);
    }
    
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationModel pagination)
    {
        return query
            .Skip((pagination.Page - 1) * pagination.Size)
            .Take(pagination.Size);
    }

    public static PaginationDto ToDto(this PaginationModel pagination, int totalCount)
    {
        return new PaginationDto
        {
            Size = pagination.Size, Current = pagination.Page,
            Count = (int)Math.Ceiling((double)totalCount / pagination.Size)
        };
    }
}