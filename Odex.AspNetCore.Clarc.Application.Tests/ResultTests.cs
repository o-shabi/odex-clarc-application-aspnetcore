using Odex.AspNetCore.Clarc.Application.Results;

namespace Odex.AspNetCore.Clarc.Application.Tests;

public class ResultTests
{
    [Fact]
    public void Ok_carries_value()
    {
        var r = Result<int>.Ok(7);
        Assert.True(r.IsSuccess);
        Assert.Equal(7, r.Value);
        Assert.Null(r.Error);
    }

    [Fact]
    public void Fail_carries_error()
    {
        var r = Result<int>.Fail(Error.Failure("x", "bad"));
        Assert.True(r.IsFailure);
        Assert.Equal("x", r.Error!.Code);
    }
}
