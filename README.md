# Csag.Extensions.Identity

ASP.NET Core Identity for applications that sign users in with tokens: Entity Framework Core stores, BCrypt password hashing, and sign-in managers that return a JSON Web Token for password and authenticator app sign-ins and accept OpenID Connect ID tokens.

| Package | Contents |
| --- | --- |
| `Csag.Extensions.Identity` | The registration (`AddCsagIdentity`), the BCrypt password hasher and the implementations of the manager interfaces |
| `Csag.Extensions.Identity.Abstractions` | `IUserManager<TUser>`, `ISignInManager<TUser>`, `ITokenSignInManager<TUser>`, `IJwtGenerator<TUser>`, the response types, the options and the OpenID Connect token types |

Both packages target .NET 10. Up to 1.0.0 they were published as `Neolution.Extensions.Identity` and `Neolution.Extensions.Identity.Abstractions`. The changelog of 2.0.0 describes the migration.

## Installation

```bash
dotnet add package Csag.Extensions.Identity
```

Projects that only consume the interfaces, for example a domain or application layer, can reference `Csag.Extensions.Identity.Abstractions` alone.

## Usage

Register the services with your `DbContext` and your user type, whose key is a `Guid`:

```csharp
services.AddCsagIdentity<AppDbContext, AppUser>();
services.AddScoped<IJwtGenerator<AppUser>, AppJwtGenerator>();
```

`AddCsagIdentity` registers ASP.NET Core Identity with the Entity Framework Core stores and the default token providers, the BCrypt password hasher, and these services:

| Service | Purpose |
| --- | --- |
| `IUserManager<TUser>` | Creates and finds users, manages passwords, claims and two-factor authentication, with logging |
| `ISignInManager<TUser>` | Checks passwords and whether a user may sign in |
| `ITokenSignInManager<TUser>` | Password and authenticator app sign-in, which return a `JsonWebToken`, OpenID Connect sign-in, which returns the user, and `CreateAccessTokenAsync` for that user |
| `IOpenIdConnectTokenFactory` | Wraps a Google ID token with its validation options, for `ITokenSignInManager<TUser>.OpenIdConnectSignInAsync` |

The application implements `IJwtGenerator<TUser>`, which turns a user and their claims into the access token.

`IdentityExtensions.RemoveLegacyClaimMappings()` clears the global claim type mappings of `JwtSecurityTokenHandler`, so JWT handlers keep the standard claim names instead of the legacy Microsoft claim types. Do not call it in an application that uses `OpenIdConnectSignInAsync`: that method reads the email address as `ClaimTypes.Email`, which only exists through the mapping, and returns `null` without it.

### Configuration

The options come from the `NeolutionIdentity` section:

```json
{
  "NeolutionIdentity": {
    "Google": {
      "ClientId": "your-client-id.apps.googleusercontent.com"
    },
    "PasswordHasher": {
      "BCryptWorkFactor": 13
    }
  }
}
```

- `Google:ClientId` is needed for `IOpenIdConnectTokenFactory.GoogleToken`.
- `PasswordHasher:BCryptWorkFactor` defaults to 13. Values below 12 fail the options validation.

## Build and Test

### Prerequisites

- The .NET SDK pinned in [global.json](global.json) (10.0.4xx, a newer 10.0 SDK rolls forward).
- Node.js 24 with Corepack, for the changesets tooling: run `corepack enable` and then `pnpm install` once in the repository root.

### Build

```bash
dotnet restore --locked-mode
dotnet build --configuration Release --no-restore
```

Every project has a `packages.lock.json`, and CI restores in locked mode. After changing a package reference, run `dotnet restore` without `--locked-mode` and commit the updated lock files.

### Pack

```bash
dotnet pack --configuration Release --no-build -o ./nupkgs
```

## Preview packages

Each pull request to `main` or to a `release/**` branch publishes both packages to a public preview feed on feedz.io. A draft PR, a PR from a fork and a PR from Dependabot publish no preview. The version is one patch above the current release, for example `2.0.1-pr.42.183` (PR 42, workflow run 183). The **"chore: version packages"** PR publishes a release candidate of the upcoming version instead, for example `2.1.0-pr.73.190`, so the changes of the next release can be tested before it ships. In prerelease mode the version in `package.json` is a prerelease such as `3.0.0-rc.0`, and the preview extends it, for example `3.0.0-rc.0.pr.42.183`. That version sorts after the prerelease and before the next one. A comment on the PR gives the version of its latest push. The feed removes previews after 30 days. Releases go to nuget.org only.

To use a preview in an application, add the feed to the `nuget.config` of the application. The feed needs no credentials:

```xml
<packageSources>
  <clear />
  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  <add key="csag-extensions-identity-previews" value="https://f.feedz.io/collana-solutions/csag-extensions-identity/nuget/index.json" />
</packageSources>
<packageSourceMapping>
  <packageSource key="nuget.org">
    <package pattern="*" />
    <package pattern="Csag.Extensions.Identity*" />
  </packageSource>
  <packageSource key="csag-extensions-identity-previews">
    <package pattern="Csag.Extensions.Identity*" />
  </packageSource>
</packageSourceMapping>
```

The `Csag.Extensions.Identity*` pattern is also necessary under nuget.org. NuGet looks for a package only in the sources of its most specific pattern, so without it the released versions do not restore.

To test a preview:

1. Set `Csag.Extensions.Identity` and `Csag.Extensions.Identity.Abstractions` to the preview version.
2. If the application restores in locked mode, update its `packages.lock.json` files.
3. Add the feed only on the branch that tests the preview. The main branch of the application then does not need feedz.io to restore.

## Release process

This repository follows the [collana-solutions release playbook](https://github.com/collana-solutions/release-playbook) (Changesets, NuGet variant):

1. Every PR that changes a package needs a changeset. Run `pnpm changeset`, pick the packages the change affects, choose the bump type and describe the change for consumers of the packages. The two packages are a fixed group, so they always share one version. A change that ships nothing, such as CI or repository documentation, needs an empty changeset: `pnpm changeset --empty`. CI's **Changeset Check** fails a PR without one.
2. On merge to `main`, the Release workflow keeps a **"chore: version packages"** PR up to date. It bumps the versions in the `package.json` files, writes them into the `.csproj` files with `scripts/sync-versions.mjs` and updates the `CHANGELOG.md` files next to the projects.
3. Merging that PR creates the git tags and the GitHub Releases. The release of `Csag.Extensions.Identity` starts the **NuGet Publish** workflow, which packs both packages and pushes them to nuget.org through trusted publishing.

Dependabot PRs get their changesets automatically. Do not edit the `CHANGELOG.md` files or the `<Version>` in the `.csproj` files by hand. The root [CHANGELOG.md](CHANGELOG.md) holds the releases up to 1.0.0.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
