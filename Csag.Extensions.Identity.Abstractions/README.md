# Csag.Extensions.Identity.Abstractions

The interfaces and types of [Csag.Extensions.Identity](https://github.com/collana-solutions/Csag.Extensions.Identity), for projects that use the identity services without depending on their implementation.

## Contents

- `IUserManager<TUser>`: creates and finds users, manages passwords, claims and two-factor authentication.
- `ISignInManager<TUser>`: checks passwords and whether a user may sign in, with `SignInResponse` and `PreSignInResponse`.
- `ITokenSignInManager<TUser>`: password and authenticator app sign-in, which return a `JsonWebToken`, OpenID Connect sign-in, which returns the user, and `CreateAccessTokenAsync` for that user.
- `IJwtGenerator<TUser>`: implemented by the application to create the access token from a user and their claims.
- `IOpenIdConnectTokenFactory`, `OpenIdConnectToken` and `OpenIdConnectTokenValidationOptions`: an ID token with the settings to validate it.
- `CsagIdentityOptions`, `GoogleOptions` and `PasswordHasherOptions`: the options bound from the `NeolutionIdentity` configuration section.

`TUser` is an `IdentityUser<Guid>`. Applications install `Csag.Extensions.Identity`, which brings this package along. The [repository README](https://github.com/collana-solutions/Csag.Extensions.Identity#readme) describes the setup.
