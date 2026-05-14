namespace Odex.AspNetCore.Clarc.Application.CQRS;

/// <summary>
/// Query base type with pagination and sorting metadata for application-layer CQRS.
/// Use <see cref="Odex.AspNetCore.Clarc.Application.Validators.PagedValidator{T}"/> when the query is bound from untrusted input (e.g. HTTP); domain invariants belong in the Domain layer.
/// </summary>
/// <typeparam name="TResponse">The handler response type.</typeparam>
public abstract record PagedQuery<TResponse> : BaseQuery<TResponse>
{
    /// <summary>
    /// Gets the one-based page index (default 1). Validated by <see cref="Odex.AspNetCore.Clarc.Application.Validators.PagedValidator{T}"/> to be at least 1.
    /// </summary>
    public int Page { get; init; } = 1;

    /// <summary>
    /// Gets the page size (default 20). Validated by <see cref="Odex.AspNetCore.Clarc.Application.Validators.PagedValidator{T}"/> to fall between 1 and 512 inclusive.
    /// </summary>
    public int PageSize { get; init; } = 20;

    /// <summary>
    /// Gets the number of rows to skip for data access (<c>(Page - 1) * PageSize</c>).
    /// </summary>
    public int SkipCount => (Page - 1) * PageSize;

    /// <summary>
    /// Gets the optional sort field name (interpretation is host-specific).
    /// </summary>
    public string? SortBy { get; init; }

    /// <summary>
    /// Gets a value indicating whether sorting is descending.
    /// </summary>
    public bool SortDescending { get; init; } = false;
}
