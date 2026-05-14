using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Odex.AspNetCore.Clarc.Application.Behaviors;
using Odex.AspNetCore.Clarc.Application.Configuration;
using Odex.AspNetCore.Clarc.Application.CQRS;
using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Tests;

public sealed record SampleQuery(string Name) : BaseQuery<bool>;

public sealed class SampleQueryValidator : AbstractValidator<SampleQuery>
{
    public SampleQueryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public sealed record SkipValidationQuery(string Name) : BaseQuery<bool>, ISkipValidation;

public sealed class SkipValidationQueryValidator : AbstractValidator<SkipValidationQuery>
{
    public SkipValidationQueryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class ValidationPipelineBehaviorTests
{
    private static ValidationPipelineBehavior<TRequest, TResponse> CreateBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators,
        ClarcValidationPipelineOptions? options = null)
        where TRequest : IRequest<TResponse> =>
        new(
            validators,
            Options.Create(options ?? new ClarcValidationPipelineOptions()),
            NullLogger<ValidationPipelineBehavior<TRequest, TResponse>>.Instance);

    [Fact]
    public async Task Handle_when_invalid_throws_application_validation_exception_with_grouped_errors()
    {
        var behavior = CreateBehavior<SampleQuery, bool>([new SampleQueryValidator()]);

        var ex = await Assert.ThrowsAsync<Odex.AspNetCore.Clarc.Application.Exceptions.ValidationException>(() =>
            behavior.Handle(new SampleQuery(""), ct => Task.FromResult(true), CancellationToken.None));

        Assert.Equal(ExceptionType.ValidationFailed, ex.Type);
        Assert.Contains(ex.Errors, pair => pair.Value.Length > 0);
        Assert.NotEmpty(ex.FailureDetails);
    }

    [Fact]
    public async Task Handle_when_valid_invokes_next()
    {
        var behavior = CreateBehavior<SampleQuery, bool>([new SampleQueryValidator()]);

        var result = await behavior.Handle(
            new SampleQuery("ok"),
            ct => Task.FromResult(true),
            CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task Handle_when_no_validators_invokes_next()
    {
        var behavior = CreateBehavior<SampleQuery, bool>([]);

        var result = await behavior.Handle(
            new SampleQuery(""),
            ct => Task.FromResult(true),
            CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task Handle_when_ISkipValidation_skips_rules()
    {
        var behavior = CreateBehavior<SkipValidationQuery, bool>([new SkipValidationQueryValidator()]);

        var result = await behavior.Handle(
            new SkipValidationQuery(""),
            ct => Task.FromResult(true),
            CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task Handle_sequential_mode_when_parallel_disabled()
    {
        var behavior = CreateBehavior<SampleQuery, bool>(
            [new SampleQueryValidator()],
            new ClarcValidationPipelineOptions { ParallelizeValidators = false });

        var result = await behavior.Handle(
            new SampleQuery("ok"),
            ct => Task.FromResult(true),
            CancellationToken.None);

        Assert.True(result);
    }
}
