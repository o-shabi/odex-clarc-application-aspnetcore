using FluentValidation;

namespace Odex.AspNetCore.Clarc.Application.Validators;

/// <summary>
/// Small reusable FluentValidation rule helpers for typical application DTOs.
/// </summary>
public static class CommonValidationExtensions
{
    /// <summary>
    /// Applies a reasonable maximum length and basic email format for string emails.
    /// </summary>
    public static IRuleBuilderOptions<T, string> RuleForApplicationEmail<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.EmailAddress().MaximumLength(320);

    /// <summary>
    /// Ensures a string is non-empty after trim and caps length.
    /// </summary>
    public static IRuleBuilderOptions<T, string> RuleForRequiredShortString<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength) =>
        ruleBuilder.NotEmpty().MaximumLength(maxLength);
}
