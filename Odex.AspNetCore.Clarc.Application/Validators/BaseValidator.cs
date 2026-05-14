using FluentValidation;

namespace Odex.AspNetCore.Clarc.Application.Validators;

/// <summary>
/// Base FluentValidation <see cref="AbstractValidator{T}"/> for application requests.
/// </summary>
/// <typeparam name="T">The validated model type.</typeparam>
public class BaseValidator<T> : AbstractValidator<T>;
