using FluentValidation;
using Odex.AspNetCore.Clarc.Application.CQRS;

namespace Odex.AspNetCore.Clarc.Application.Validators;

/// <summary>
/// FluentValidation rules for <see cref="PagedQuery{TResponse}"/> (page ≥ 1, page size 1–512).
/// </summary>
/// <typeparam name="T">The query response type (unused for rules; required for <see cref="PagedQuery{TResponse}"/>).</typeparam>
public class PagedValidator<T> : AbstractValidator<PagedQuery<T>>
{
    /// <summary>
    /// Initializes a new instance with default pagination bounds.
    /// </summary>
    public PagedValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 512);
    }
}
