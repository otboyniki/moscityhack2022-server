using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Web.Extensions;

namespace Web.ViewModels;

public class Page<T>
{
    public int Total { get; set; }

    [JsonPropertyName("page")]
    public int PageNumber { get; set; }

    public int? Limit { get; set; }

    public ICollection<T> Data { get; set; } = Array.Empty<T>();
}

public interface IPaginationRequest
{
    public int Page { get; }
    public int? Limit { get; }
}

public static class PageExtensions
{
    public static async Task<Page<T>> PaginateAsync<T>(this IQueryable<T> query, IPaginationRequest request,
                                                       CancellationToken cancellationToken)
    {
        var total = await query.CountAsync(cancellationToken);

        if (request.Limit.HasValue)
        {
            var page = Math.Max(1, request.Page);
            var limit = Math.Max(1, request.Limit.Value);

            query = query.Skip(limit * (page - 1))
                         .Take(limit);
        }

        return new Page<T>
        {
            Total = total,
            PageNumber = request.Limit.HasValue
                ? request.Page.Let(x => Math.Max(1, x))
                : 1,
            Limit = request.Limit?.Let(x => Math.Max(1, x)) ?? total,
            Data = await query.ToListAsync(cancellationToken)
        };
    }
}