using Odex.AspNetCore.Clarc.Application.CQRS;
using Odex.AspNetCore.Clarc.Application.Validators;

namespace Odex.AspNetCore.Clarc.Application.Tests;

public sealed record SamplePagedQuery : PagedQuery<bool>;

public class PagedValidatorTests
{
    [Fact]
    public async Task Page_below_one_fails()
    {
        var v = new PagedValidator<bool>();
        var result = await v.ValidateAsync(new SamplePagedQuery { Page = 0, PageSize = 10 });
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task Page_size_above_512_fails()
    {
        var v = new PagedValidator<bool>();
        var result = await v.ValidateAsync(new SamplePagedQuery { Page = 1, PageSize = 513 });
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task Default_bounds_pass()
    {
        var v = new PagedValidator<bool>();
        var result = await v.ValidateAsync(new SamplePagedQuery());
        Assert.True(result.IsValid);
    }
}
