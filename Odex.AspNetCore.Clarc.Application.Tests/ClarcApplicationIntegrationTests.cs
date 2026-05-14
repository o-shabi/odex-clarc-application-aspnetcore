using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Odex.AspNetCore.Clarc.Application;
using Odex.AspNetCore.Clarc.Application.CQRS;

namespace Odex.AspNetCore.Clarc.Application.Tests;

public sealed record EchoQuery(string Text) : BaseQuery<string>;

public sealed class EchoQueryHandler : IRequestHandler<EchoQuery, string>
{
    public Task<string> Handle(EchoQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(request.Text);
}

public class ClarcApplicationIntegrationTests
{
    [Fact]
    public async Task AddClarcApplication_resolves_mediator_and_runs_handler()
    {
        var services = new ServiceCollection();
        services.AddClarcApplication(b => b.AddAssemblyContaining<EchoQueryHandler>());
        await using var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        var result = await mediator.Send(new EchoQuery("hi"));

        Assert.Equal("hi", result);
    }
}
