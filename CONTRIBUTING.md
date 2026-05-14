# Contributing

Thank you for helping improve **Odex.AspNetCore.Clarc.Application**. This project follows common practices for open-source .NET libraries on GitHub and NuGet.

## Before you start

- Read [README.md](README.md) for scope: **application-layer CQRS** with **MediatR** and **FluentValidation** (no dependency on **Odex.AspNetCore.Clarc.Domain**; add Domain in your host when needed).
- Check [CHANGELOG.md](CHANGELOG.md) and [open issues](https://github.com/o-shabi/odex-clarc-application-aspnetcore/issues) to avoid duplicate work.
- For **security-sensitive** reports, use [SECURITY.md](SECURITY.md) instead of a public issue.

## Development setup

Requirements:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download) (see [global.json](global.json) for the preferred feature band).

Commands:

```bash
git clone https://github.com/o-shabi/odex-clarc-application-aspnetcore.git
cd odex-clarc-application-aspnetcore
dotnet restore Odex.AspNetCore.Clarc.Application.sln
dotnet build Odex.AspNetCore.Clarc.Application.sln -c Release
dotnet test Odex.AspNetCore.Clarc.Application.sln -c Release
```

Strict build (matches CI):

```bash
dotnet build Odex.AspNetCore.Clarc.Application.sln -c Release -p:TreatWarningsAsErrors=true -p:ContinuousIntegrationBuild=true
```

The **`ContinuousIntegrationBuild`** flag enables NuGet vulnerability audit (see **`Directory.Build.props`**). Omit it for offline restores against a local package cache only.

Verify the NuGet layout locally:

```bash
dotnet pack Odex.AspNetCore.Clarc.Application/Odex.AspNetCore.Clarc.Application.csproj -c Release -o ./artifacts
```

The output includes **`Odex.AspNetCore.Clarc.Application.xml`** (API documentation), the `.dll`, and optionally **`.snupkg`**.

## Pull requests

- Keep changes focused; match existing formatting and naming.
- Add or update **tests** in **Odex.AspNetCore.Clarc.Application.Tests** when behavior changes.
- Public API changes require **`///` XML documentation** so CI can use **`TreatWarningsAsErrors`** with **`GenerateDocumentationFile`**.
- Note user-visible changes in [CHANGELOG.md](CHANGELOG.md) under **Unreleased**.

## License

By contributing, you agree that your contributions are licensed under the same terms as the project ([LICENSE](LICENSE)).
