# Csag.Extensions.Identity

ASP.NET Core Identity for applications that sign users in with tokens: Entity Framework Core stores, BCrypt password hashing, and sign-in managers that return a JSON Web Token for password and authenticator app sign-ins and accept OpenID Connect ID tokens.

## Installation

```bash
dotnet add package Csag.Extensions.Identity
```

## Usage

Register the services with your `DbContext` and your user type, whose key is a `Guid`, and your implementation of `IJwtGenerator<TUser>`:

```csharp
services.AddCsagIdentity<AppDbContext, AppUser>();
services.AddScoped<IJwtGenerator<AppUser>, AppJwtGenerator>();
```

Then inject `IUserManager<AppUser>`, `ISignInManager<AppUser>` or `ITokenSignInManager<AppUser>`:

```csharp
var token = await tokenSignInManager.PasswordSignInAsync(email, password);
```

The options come from the `NeolutionIdentity` configuration section, with `Google:ClientId` for Google ID tokens and `PasswordHasher:BCryptWorkFactor` (default 13, at least 12).

The [repository README](https://github.com/collana-solutions/Csag.Extensions.Identity#readme) describes the registered services and the configuration.
