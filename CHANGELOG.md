# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed

- **`PagedValidator<T>`** now inherits **`BaseValidator<PagedQuery<T>>`** instead of **`AbstractValidator<PagedQuery<T>>`** directly, matching other application validators.

## [0.2.0] - 2026-05-15

### Added

- **`ClarcApplicationBuilder`** / **`ClarcApplicationDescriptor`** and **`AddClarcApplication(Action<ClarcApplicationBuilder>)`** for multi-assembly scanning, MediatR customization, validator lifetime, and toggling individual pipeline behaviors.
- **`ClarcValidationPipelineOptions`** (rule sets, parallel vs sequential validators, optional failure logging) and **`ClarcLoggingPipelineOptions`** (log level, include type name).
- Pipeline behaviors: **`LoggingPipelineBehavior`**, **`RequestMetricsPipelineBehavior`** (`System.Diagnostics.Metrics`), **`UnhandledExceptionLoggingPipelineBehavior`** (logs and rethrows).
- **`BaseCommand`** (non-generic, **`IRequest<Unit>`**) for commands without a typed response; **`ICommand`**, **`IQuery`**, **`ISkipValidation`** markers.
- **`PagedQuery.SkipCount`**; **`Result<T>`** / **`Error`** for non-exception result flows.
- **`ValidationFailureDetail`**, **`ValidationException.FromFailures`**, and **`FailureDetails`** on **`ValidationException`**.
- **`ApplicationException.ErrorCode`** and optional inner exception support on the base type.
- New exceptions: **`NotFoundException`**, **`ConflictException`**, **`GoneException`**, **`ResourceTimeoutException`**, **`TooManyRequestsException`**; extended **`ExceptionType`** enum.
- **`CommonValidationExtensions`** for small reusable FluentValidation helpers.
- **`.editorconfig`**, **`Odex.AspNetCore.Clarc.Application.Tests`** (xUnit): validation pipeline, **`PagedValidator`**, integration (**`IMediator`**), builder, **`ISkipValidation`**, **`Result<T>`**, paging, exception smoke tests.
- Repository tooling: root **`README.md`**, **`LICENSE`**, **`CONTRIBUTING.md`**, **`SECURITY.md`**, **`CODE_OF_CONDUCT.md`**, **`Directory.Build.props`**, **`.gitignore`**, GitHub Actions (**`ci.yml`**, **`release.yml`**), Dependabot, issue templates, PR template.

### Changed

- **`PagedResponse<T>.Items`** is **`IReadOnlyList<T>`** (immutable surface for consumers).
- **`PagedValidator<T>`:** page **≥ 1**; page size **1–512** (aligned with typical API/domain paging bounds).
- **`ServiceUnavailableException`** message: “not available **at** the moment”.
- **`ServiceInternalException`** message uses **“threw”** wording.
- **`ApplicationException`** uses a protected constructor with optional **`innerException`** and **`ErrorCode`** (derived primary constructors unchanged at call sites).
- **`AddClarcApplication`** registers **`AddLogging()`**, **`IOptions<T>`** for pipeline options, default pipeline behaviors (see README for order), and **`AddValidatorsFromAssemblies`** when using the builder overload.
- **`ValidationPipelineBehavior`** uses **`IOptions<ClarcValidationPipelineOptions>`**, **`ILogger<>`**, supports **`ISkipValidation`**, optional FluentValidation rule sets, parallel or sequential validator execution, and structured **`ValidationException`** failures.

### Removed

- **`Odex.AspNetCore.Clarc.Domain`** package dependency and **`DomainPagedRequestValidator`** — domain types own invariants; this layer validates application DTOs (e.g. **`PagedQuery<T>`** via **`PagedValidator<T>`**).

### Fixed

- **`ValidationPipelineBehavior`** now throws **`Odex.AspNetCore.Clarc.Application.Exceptions.ValidationException`** with grouped **`Errors`** (and **`FailureDetails`**), not **`FluentValidation.ValidationException`** from ambiguous constructor resolution.

### Build

- **`Directory.Build.props`:** NuGet vulnerability audit when **`ContinuousIntegrationBuild=true`** (CI); off by default for local builds to avoid **NU1900** when **api.nuget.org** is unreachable.
- **SourceLink:** **`Microsoft.SourceLink.GitHub`** for richer debugging when built from a Git working tree.

## [0.1.1]

Prior releases: see NuGet version history and git tags.
