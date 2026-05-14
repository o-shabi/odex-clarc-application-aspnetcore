# Odex.AspNetCore.Clarc.Application

[![NuGet Version](https://img.shields.io/nuget/v/Odex.AspNetCore.Clarc.Application)](https://www.nuget.org/packages/Odex.AspNetCore.Clarc.Application)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Odex.AspNetCore.Clarc.Application)](https://www.nuget.org/packages/Odex.AspNetCore.Clarc.Application)
[![CI](https://github.com/o-shabi/odex-clarc-application-aspnetcore/actions/workflows/ci.yml/badge.svg)](https://github.com/o-shabi/odex-clarc-application-aspnetcore/actions/workflows/ci.yml)
[![Release](https://github.com/o-shabi/odex-clarc-application-aspnetcore/actions/workflows/release.yml/badge.svg)](https://github.com/o-shabi/odex-clarc-application-aspnetcore/actions/workflows/release.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**Application layer for CQRS‑based ASP.NET Core applications**  
*Provides MediatR pipelines, CQRS abstractions, FluentValidation integration, and typed application exceptions.*

---

## 📦 Overview

`Odex.AspNetCore.Clarc.Application` is the application layer component of the Clarc framework. It bridges the domain and
infrastructure layers by implementing **CQRS** (Command Query Responsibility Segregation) patterns using **MediatR** and
**FluentValidation**. It provides:

- **CQRS Abstractions** – Base records for commands, queries, and responses.
- **Validation Pipeline** – Automatic validation of requests using FluentValidation.
- **Paged Requests & Responses** – Reusable pagination and sorting models.
- **Application Exceptions** – Typed exceptions for validation, duplicates, service failures, authorization, etc.
- **DI Extensions** – One‑line registration of MediatR, validators, and pipeline behaviors.

---

## 🚀 Get Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- An ASP.NET Core project
- **[Odex.AspNetCore.Clarc.Domain](https://www.nuget.org/packages/Odex.AspNetCore.Clarc.Domain)** in your solution when handlers use aggregates, repositories, or domain value objects (this application package does not reference Domain).

### Installation

```bash
dotnet add package Odex.AspNetCore.Clarc.Application
```

Or using the Package Manager Console:

```bash
Install-Package Odex.AspNetCore.Clarc.Application
```

### Basic Setup

**1. Register the Application Layer in `Program.cs`**

```csharp
using Odex.AspNetCore.Clarc.Application;

var builder = WebApplication.CreateBuilder(args);

// Register MediatR, validators, and validation pipeline
builder.Services.AddClarcApplication<Program>(); // T is any type from your assembly

// ... rest of your configuration
```

**2. Create a Simple Query and Handler**

```csharp
using MediatR;
using Odex.AspNetCore.Clarc.Application.CQRS;

public record GetUserQuery(int Id) : BaseQuery<UserResponse>;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserResponse>
{
    public async Task<UserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        // Fetch user from repository
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        return new UserResponse { Id = user.Id, Name = user.Name };
    }
}
```

**3. Create a Validator for the Query**

```csharp
using FluentValidation;
using Odex.AspNetCore.Clarc.Application.Validators;

public class GetUserQueryValidator : BaseValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("User ID must be positive");
    }
}
```

**4. Send the Query from a Controller**

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _mediator.Send(new GetUserQuery(id));
        return Ok(result);
    }
}
```

### Minimal API Example

```csharp
app.MapGet("/users/{id}", async (IMediator mediator, int id) =>
{
    var user = await mediator.Send(new GetUserQuery(id));
    return Results.Ok(user);
});
```

### Advanced registration

Use **`ClarcApplicationBuilder`** when you scan multiple assemblies, customize MediatR, toggle pipeline behaviors, or set validator lifetime:

```csharp
using Odex.AspNetCore.Clarc.Application;
using Odex.AspNetCore.Clarc.Application.Configuration;

builder.Services.AddClarcApplication(b =>
{
    b.AddAssemblyContaining<Program>()
        .AddAssembly(typeof(SomeOtherHandlers).Assembly)
        .ConfigureMediatR(cfg => { /* optional */ })
        .UseValidatorLifetime(ServiceLifetime.Scoped)
        .ConfigureValidation(v => { v.RuleSet = "Write"; v.LogFailures = true; })
        .EnableMetricsPipeline(true);
});
```

Pipeline order (outer → inner): **unhandled-exception logging**, **request logging**, **metrics**, **FluentValidation**. **`AddClarcApplication`** calls **`AddLogging()`** so **`ILogger<>`** resolves.

### Security note

Pipeline loggers **never** write request payloads. Treat application DTOs as untrusted until validated; keep domain invariants inside the Domain model.

---

## ✨ Features

| Feature                       | Description                                                                                                                                     |
|-------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------|
| 🧩 **CQRS Base Records**      | `BaseCommand` / `BaseCommand<TResponse>`, `BaseQuery<TResponse>`, `ICommand` / `IQuery`, `BaseResponse` with timestamps. |
| 📄 **Paged Support**          | `PagedQuery<TResponse>` (includes **`SkipCount`**) / `PagedResponse<T>` with **`IReadOnlyList<T>`** items; **`PagedValidator<T>`** for untrusted input. |
| ✅ **Validation Pipeline**     | `ValidationPipelineBehavior` with rule sets, parallel/sequential validators, **`ISkipValidation`** opt-out, structured **`ValidationFailureDetail`**. |
| ⚠️ **Application Exceptions** | Typed exceptions including **`NotFoundException`**, **`ConflictException`**, **`GoneException`**, **`ResourceTimeoutException`**, **`TooManyRequestsException`**, plus **`ErrorCode`** on **`ApplicationException`**. |
| 📊 **Observability**          | Optional **`LoggingPipelineBehavior`**, **`RequestMetricsPipelineBehavior`** (`System.Diagnostics.Metrics`), **`UnhandledExceptionLoggingPipelineBehavior`**. |
| 🔌 **Registration**           | **`AddClarcApplication<T>()`** or **`AddClarcApplication(Action<ClarcApplicationBuilder>)`**. |
| 🧾 **Result model**           | **`Result<T>`** / **`Error`** for APIs that prefer non-exception flows. |

---

## 🏗️ Core Components

### 1. CQRS Abstractions

**BaseCommand<TResponse>**

```csharp
using MediatR;

namespace Odex.AspNetCore.Clarc.Application.CQRS;

public abstract record BaseCommand<TResponse> : IRequest<TResponse>, ICommand
{
    public DateTime ExecutedAt { get; init; } = DateTime.UtcNow;
}
```

**BaseQuery<TResponse>**

```csharp
using MediatR;

namespace Odex.AspNetCore.Clarc.Application.CQRS;

public abstract record BaseQuery<TResponse> : IRequest<TResponse>, IQuery
{
    public DateTime RequestedAt { get; init; } = DateTime.UtcNow;
}
```

**BaseResponse**

```csharp
namespace Odex.AspNetCore.Clarc.Application.CQRS;

public abstract record BaseResponse
{
    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
}
```

**PagedQuery<TResponse>**

```csharp
namespace Odex.AspNetCore.Clarc.Application.CQRS;

public abstract record PagedQuery<TResponse> : BaseQuery<TResponse>
{
    #region Pagination
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public int SkipCount => (Page - 1) * PageSize;
    #endregion

    #region Sorting
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; } = false;
    #endregion
}
```

**PagedResponse<T>**

```csharp
namespace Odex.AspNetCore.Clarc.Application.CQRS;

public abstract record PagedResponse<T> : BaseResponse
{
    public required IReadOnlyList<T> Items { get; init; }
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
```

### 2. Validation Pipeline

**ValidationPipelineBehavior<TRequest, TResponse>**

```csharp
using FluentValidation;
using MediatR;

namespace Odex.AspNetCore.Clarc.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errors = failures
                .GroupBy(f => string.IsNullOrEmpty(f.PropertyName) ? string.Empty : f.PropertyName)
                .ToDictionary(
                    g => string.IsNullOrEmpty(g.Key) ? "_" : g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            throw new Odex.AspNetCore.Clarc.Application.Exceptions.ValidationException(errors);
        }

        return await next(cancellationToken);
    }
}
```

### 3. Application Exceptions

**Exception Types Enum:**

```csharp
namespace Odex.AspNetCore.Clarc.Application.Enums;

public enum ExceptionType
{
    Unknown,
    Duplicate,
    ServiceFailed,
    ServiceUnavailable,
    OperationDenied,
    UnallowedOperation,
    AccessDenied,
    ValidationFailed,
    NotFound,
    Conflict,
    Gone,
    Timeout,
    TooManyRequests
}
```

**Base Exception:**

```csharp
using Odex.AspNetCore.Clarc.Application.Enums;

namespace Odex.AspNetCore.Clarc.Application.Exceptions;

public abstract class ApplicationException : Exception
{
    public ExceptionType Type { get; }
    public string? ErrorCode { get; }
    protected ApplicationException(string message, ExceptionType type, Exception? innerException = null, string? errorCode = null)
        : base(message, innerException) { Type = type; ErrorCode = errorCode; }
}
```

**Concrete Exceptions:**

| Exception                      | Code                                                                                                                                                                                                                                       |
|--------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `DuplicateResourceException`   | `public class DuplicateResourceException(string resource, string identifier) : ApplicationException($"{resource} with identifier '{identifier}' already exists", ExceptionType.Duplicate);`                                                |
| `OperationDeniedException`     | `public class OperationDeniedException(string operation, string reason = "No reason provided") : ApplicationException($"Operation '{operation}' denied due to security reasons: {reason}", ExceptionType.OperationDenied);`                |
| `OperationNotAllowedException` | `public class OperationNotAllowedException(string operation, string reason = "No reason provided") : ApplicationException($"Operation '{operation}' is not allowed: {reason}", ExceptionType.UnallowedOperation);`                         |
| `ServiceInternalException`     | `ServiceInternalException` — service failure with **`ErrorCode`** **`service_failed`**. |
| `ServiceUnavailableException`  | `ServiceUnavailableException` — temporary unavailability with **`ErrorCode`** **`service_unavailable`**. |
| `UnauthorizedAccessException`  | `UnauthorizedAccessException` — access denied with **`ErrorCode`** **`access_denied`**. |
| `ValidationException`          | Carries **`Errors`** and structured **`FailureDetails`**; use **`ValidationException.FromFailures`** from custom pipelines if needed. |
| `NotFoundException` | **`NotFoundException`** with **`ErrorCode`** **`not_found`**. |
| `ConflictException` | **`ConflictException`** with **`ErrorCode`** **`conflict`**. |
| `TooManyRequestsException` | **`TooManyRequestsException`** with **`ErrorCode`** **`too_many_requests`**. |

### 4. Validators

**BaseValidator<T>**

```csharp
using FluentValidation;

namespace Odex.AspNetCore.Clarc.Application.Validators;

public class BaseValidator<T> : AbstractValidator<T>;
```

**PagedValidator<T>**

```csharp
using FluentValidation;
using Odex.AspNetCore.Clarc.Application.CQRS;

namespace Odex.AspNetCore.Clarc.Application.Validators;

public class PagedValidator<T> : AbstractValidator<PagedQuery<T>>
{
    public PagedValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 512);
    }
}
```

### 5. Service registration

The library registers **MediatR**, **FluentValidation**, **`Microsoft.Extensions.Logging`**, **`IOptions<T>`** for pipeline settings, and (by default) four **`IPipelineBehavior<,>`** implementations.

Use **`AddClarcApplication<T>()`** for single-assembly apps, or **`AddClarcApplication(Action<ClarcApplicationBuilder>)`** for full control (see **Advanced registration** above). See source for **`ServiceCollectionExtensions`**.

---

## 🚀 Usage Examples

### Creating a Command

```csharp
public record CreateUserCommand(string Name, string Email) : BaseCommand<UserResponse>;
```

### Creating a Query Handler

```csharp
public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    
    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
            throw new NotFoundException(nameof(User), request.Id);
            
        return new UserResponse(user.Id, user.Name, user.Email);
    }
}
```

### Creating a Validator

```csharp
public class CreateUserCommandValidator : BaseValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
```

### Using Paged Query

```csharp
public record GetUsersPagedQuery : PagedQuery<PagedResponse<UserResponse>>;

public class GetUsersPagedQueryHandler : IRequestHandler<GetUsersPagedQuery, PagedResponse<UserResponse>>
{
    public async Task<PagedResponse<UserResponse>> Handle(GetUsersPagedQuery request, CancellationToken ct)
    {
        var users = await _userRepository.GetPagedAsync(request.Page, request.PageSize, ct);
        var total = await _userRepository.CountAsync(ct);
        
        return new PagedResponse<UserResponse>
        {
            Items = users.Select(u => new UserResponse(u.Id, u.Name)).ToList(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
```

### Throwing Application Exceptions

```csharp
if (await _userRepository.EmailExistsAsync(command.Email))
    throw new DuplicateResourceException("User", command.Email);

if (!_authorizationService.CanDelete(userId, currentUserId))
    throw new UnauthorizedAccessException("DeleteUser", "Insufficient permissions");

try
{
    await _emailService.SendAsync(user.Email);
}
catch (HttpRequestException)
{
    throw new ServiceUnavailableException("EmailService");
}
```

---

## 📂 Namespace Map

| Namespace                                      | Purpose                              |
|------------------------------------------------|--------------------------------------|
| `Odex.AspNetCore.Clarc.Application.Behaviors`  | Pipeline behaviors (validation, logging, metrics, unhandled exception logging) |
| `Odex.AspNetCore.Clarc.Application.Configuration` | `ClarcApplicationBuilder`, `ClarcApplicationDescriptor`, options types |
| `Odex.AspNetCore.Clarc.Application.CQRS`       | Base command/query/response records, markers (`ICommand`, `IQuery`, `ISkipValidation`) |
| `Odex.AspNetCore.Clarc.Application.Enums`      | `ExceptionType` enum                 |
| `Odex.AspNetCore.Clarc.Application.Exceptions` | Application-specific exceptions      |
| `Odex.AspNetCore.Clarc.Application.Results`    | `Result<T>`, `Error`                   |
| `Odex.AspNetCore.Clarc.Application.Validators` | `BaseValidator<T>`, `PagedValidator<T>`, `CommonValidationExtensions` |
| `Odex.AspNetCore.Clarc.Application`            | `ServiceCollectionExtensions`        |

---

## 🔗 Related Packages

- **Odex.AspNetCore.Clarc.Domain** – Domain layer (add in your host when handlers use domain types).
- **Odex.AspNetCore.Clarc.Infrastructure** – Query builders, pagination extensions, and infrastructure exceptions (optional in your host).

---

## 🤝 Contributing

Contributions are welcome! Please follow the standard GitHub flow:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

See [CONTRIBUTING.md](CONTRIBUTING.md) for maintainer expectations.

---

## 📄 License

This project is licensed under the **MIT License** – see the [LICENSE](LICENSE) file for details.

---

*Built with ❤️ for clean CQRS and validation on ASP.NET Core*
