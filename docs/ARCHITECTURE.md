# Architecture decision: modular monolith + vertical slices

Status: accepted for the starter.

One ASP.NET Core deployment and one SQL Server database. Accounts, Templates and Invitations own separate schemas and DbContexts. No module references another module implementation. The host is the composition root and may reference all modules. The browser references Contracts only; server assemblies and connection strings never enter its project graph.

Within Invitations, each feature lives under Features/<UseCase>. Domain rules belong to InvitationDraft and are independent of HTTP/EF. Persistence mapping belongs to Infrastructure. Simple slices may use their module DbContext directly; do not introduce generic repositories, a mediator, or event buses until a concrete need exists.

Templates exposes ITemplateCatalog; Invitations consumes that contract. The initial catalog is a small EF model, not a general-purpose template engine. The initial snapshot includes template version and accent only. Before publishing cards, implement immutable template revision assets and an explicit published snapshot. A rowversion exists, but optimistic editing is not implemented yet.

Accounts uses standard ASP.NET Core Identity; its UI adapters live in the Web host because cookies and SSR account forms depend on HTTP. This is an explicit adapter boundary, not a reason to place business rules in UI components.

## Request flow

CreateDraft: authenticated browser -> CSRF validation -> request validation -> template contract -> domain creation -> SQL save -> 201.
GetDraft: authenticated principal -> IDraftReader -> predicate containing both Id and OwnerId -> 200/404. Never accept owner identifiers from request bodies. UI authorization is convenience; server checks are authoritative.

CardPreview is shared between live browser editing and the private SSR draft page. Current names/message are Razor-encoded text, never raw user HTML. Future SVG uploads require sanitization and media URLs must be validated. Client validation is for usability, server validation is authoritative.

## Rules for the next feature

1. Choose the module that owns the data.
2. Add a named feature directory and a small request/response contract if it crosses a boundary.
3. Keep entities and EF types out of Contracts and Client.
4. Validate authorization/ownership on every private endpoint and CSRF on every cookie-authenticated mutation.
5. Add EF mapping and a migration to the owning module when needed.
6. Add tests for the business risk: ownership, money, state transitions or concurrency.
7. Document failures and observable behavior, not just a happy path.

Do not add empty module projects for planned capabilities. Guests, Billing, Media and Delivery will be introduced when their first real feature is implemented. Do not join other modules' tables or share tracked entities across contexts. Future payment/publishing workflows must explicitly handle cross-module consistency; no distributed transaction or outbox is present today.

## Technology baseline

.NET 10, Blazor Web App, per-page Interactive WebAssembly, EF Core SQL Server. Microsoft Identity account components originate from the official .NET SDK Blazor template and were adapted to the Accounts module. See https://learn.microsoft.com/aspnet/core/blazor/ and https://learn.microsoft.com/ef/core/providers/sql-server/ . Package versions are pinned in Directory.Packages.props; inspect upgrades rather than floating versions.
