using Odex.AspNetCore.Clarc.Application.CQRS;

namespace Odex.AspNetCore.Clarc.Application.Tests;

public sealed record PageMathQuery : PagedQuery<bool>;

public class PagedQueryTests
{
    [Fact]
    public void SkipCount_matches_page_and_size()
    {
        var q = new PageMathQuery { Page = 3, PageSize = 10 };
        Assert.Equal(20, q.SkipCount);
    }
}
