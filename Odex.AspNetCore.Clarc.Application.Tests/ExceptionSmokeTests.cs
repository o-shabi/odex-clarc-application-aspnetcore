using Odex.AspNetCore.Clarc.Application.Enums;
using Odex.AspNetCore.Clarc.Application.Exceptions;

namespace Odex.AspNetCore.Clarc.Application.Tests;

public class ExceptionSmokeTests
{
    [Fact]
    public void NotFoundException_has_type_and_code()
    {
        var ex = new NotFoundException("User", "42");
        Assert.Equal(ExceptionType.NotFound, ex.Type);
        Assert.Equal("not_found", ex.ErrorCode);
    }

    [Fact]
    public void ConflictException_has_type()
    {
        var ex = new ConflictException("version mismatch");
        Assert.Equal(ExceptionType.Conflict, ex.Type);
    }
}
