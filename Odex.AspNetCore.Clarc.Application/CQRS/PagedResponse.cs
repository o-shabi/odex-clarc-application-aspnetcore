namespace Odex.AspNetCore.Clarc.Application.CQRS;

/// <summary>
/// Application-layer paged response with navigation helpers (<c>Items</c>, <c>TotalCount</c>, <c>Page</c>, <c>PageSize</c>).
/// This shape is separate from a domain-level <c>PagedResponse&lt;T&gt;</c> that might use <c>Data</c> and <c>Total</c> naming.
/// </summary>
/// <typeparam name="T">Element type of the current page.</typeparam>
public abstract record PagedResponse<T> : BaseResponse
{
    /// <summary>
    /// Gets the items returned for the current page.
    /// </summary>
    public required IReadOnlyList<T> Items { get; init; }

    /// <summary>
    /// Gets the total number of items matching the query across all pages.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the one-based page index for this payload.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Gets the page size for this payload.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of pages given <see cref="TotalCount"/> and <see cref="PageSize"/>.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Gets a value indicating whether a previous page exists.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Gets a value indicating whether a next page exists.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;
}
