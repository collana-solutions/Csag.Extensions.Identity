---
"@collana-solutions/csag-extensions-identity": major
"@collana-solutions/csag-extensions-identity-abstractions": major
---

The packages move to new IDs and namespaces, and the registration method and the types that carried the old name follow. The behavior is the same as in 1.0.0.

**New package IDs:**

| Up to 1.0.0 | From 2.0.0 |
| --- | --- |
| `Neolution.Extensions.Identity` | `Csag.Extensions.Identity` |
| `Neolution.Extensions.Identity.Abstractions` | `Csag.Extensions.Identity.Abstractions` |

**To migrate:**

1. Replace the package references as in the table above, at 2.0.0 or later.
2. Change the namespaces: `using Neolution.Extensions.Identity;` becomes `using Csag.Extensions.Identity;`, and the same for `.Abstractions`, `.Abstractions.OpenIdConnect` and `.Abstractions.Options`. `IdentityExtensions` stays in the `Microsoft.Extensions.DependencyInjection` namespace.
3. Rename the registration method and the options types:

   | Up to 1.0.0 | From 2.0.0 |
   | --- | --- |
   | `AddNeolutionIdentity<TDbContext, TUserAccount>()` | `AddCsagIdentity<TDbContext, TUserAccount>()` |
   | `NeolutionIdentityOptions` | `CsagIdentityOptions` |
   | `NeolutionIdentityOptionsValidator` | `CsagIdentityOptionsValidator` |

4. The log categories follow the namespaces. If your logging configuration sets a level for `Neolution.Extensions.Identity`, set it for `Csag.Extensions.Identity` instead.

**Unchanged:** the configuration section keeps its name `NeolutionIdentity`, so app settings, environment variables and secrets need no change. Stored data needs no migration either: the Entity Framework Core stores, the BCrypt password hashes, the token providers, the claim types and the `amr` and authentication method values in the access tokens are the same as in 1.0.0.

The `Neolution.Extensions.Identity` and `Neolution.Extensions.Identity.Abstractions` IDs get deprecated on nuget.org, with the `Csag.Extensions.Identity` packages as the alternates. Their existing versions stay available, but they get no new releases.
