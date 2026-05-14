using Odex.AspNetCore.Clarc.Application.Configuration;

namespace Odex.AspNetCore.Clarc.Application.Tests;

public class ClarcApplicationBuilderTests
{
    [Fact]
    public void Build_throws_when_no_assemblies()
    {
        var builder = new ClarcApplicationBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_succeeds_with_assembly()
    {
        var d = new ClarcApplicationBuilder()
            .AddAssemblyContaining<ClarcApplicationBuilderTests>()
            .EnableValidationPipeline(false)
            .EnableLoggingPipeline(false)
            .EnableMetricsPipeline(false)
            .EnableUnhandledExceptionLoggingPipeline(false)
            .Build();

        Assert.NotEmpty(d.Assemblies);
    }
}
