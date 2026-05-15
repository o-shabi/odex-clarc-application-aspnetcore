using FluentValidation;

namespace Odex.AspNetCore.Clarc.Application.Validators;

/// <summary>
/// Base FluentValidation <see cref="AbstractValidator{T}"/> for application requests.
/// Subclass for custom rules; <see cref="PagedValidator{T}"/> extends this type for <see cref="CQRS.PagedQuery{TResponse}"/>.
/// </summary>
/// <typeparam name="T">The validated model type.</typeparam>
public class BaseValidator<T> : AbstractValidator<T>;
