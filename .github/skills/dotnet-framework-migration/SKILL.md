---
name: dotnet-framework-migration
description: Knowledge base on migrating .NET Framework 4.x applications to .NET 10. Use when planning or executing migrations from .NET Framework to .NET 10.
---

# .NET Framework → .NET 10 Migration

## Project File

**Before** (full `.csproj` with packages.config)  
**After** (SDK-style `.csproj`):

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.*" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.*" />
  </ItemGroup>
</Project>
```

---

## Package Mappings

| .NET Framework | .NET 10 |
|---|---|
| `System.Web.Http` (ASP.NET Web API 2) | `Microsoft.AspNetCore.Mvc` |
| `EntityFramework` (EF6) | `Microsoft.EntityFrameworkCore.SqlServer` |
| `System.Data.SqlClient` | `Microsoft.Data.SqlClient` |
| `System.Configuration.ConfigurationManager` | `Microsoft.Extensions.Configuration` (built-in) |
| `Unity` / `Autofac` IoC | Built-in `Microsoft.Extensions.DependencyInjection` |

---

## Startup & Configuration

**Remove:** `Global.asax`, `Global.asax.cs`, `WebApiConfig.cs`, `Web.config`  
**Add:** `Program.cs`, `appsettings.json`

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<PartsCatalogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=PartsCatalog;Trusted_Connection=True;"
  }
}
```

> **Breaking change:** Middleware order matters. `UseAuthentication()` must appear before `UseAuthorization()`.

---

## Controllers

| Aspect | .NET Framework | .NET 10 |
|---|---|---|
| Base class | `ApiController` | `ControllerBase` |
| Namespace | `System.Web.Http` | `Microsoft.AspNetCore.Mvc` |
| Return type | `IHttpActionResult` | `ActionResult<T>` |
| Routing | Convention-based in `WebApiConfig` | Attribute-based (`[Route]`, `[HttpGet]`) |
| DI | Manual / service locator | Constructor injection |
| Data access | Synchronous | Async (`await`) required |

```csharp
// Before (.NET Framework)
public class ProductsController : ApiController
{
    private PartsCatalogContext db = new PartsCatalogContext();

    public IHttpActionResult GetProducts()
    {
        var products = db.Products.Include("Category").ToList();
        return Ok(products);
    }
}

// After (.NET 10)
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly PartsCatalogContext _context;

    public ProductsController(PartsCatalogContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        => await _context.Products.Include(p => p.Category).ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return product is null ? NotFound() : product;
    }
}
```

> **Breaking change:** `[FromBody]` is now inferred for complex types when `[ApiController]` is applied, but be explicit for clarity.

---

## Configuration Access

```csharp
// Before
string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

// After — inject IConfiguration or rely on EF Core's registered DbContext
public class MyService(IConfiguration config)
{
    string conn = config.GetConnectionString("DefaultConnection");
}
```

---

## Entity Framework 6 → EF Core

### DbContext

```csharp
// Before (EF6)
public class PartsCatalogContext : DbContext
{
    public PartsCatalogContext() : base("name=DefaultConnection") { }
    protected override void OnModelCreating(DbModelBuilder modelBuilder) { ... }
}

// After (EF Core)
public class PartsCatalogContext : DbContext
{
    public PartsCatalogContext(DbContextOptions<PartsCatalogContext> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

### Key EF6 → EF Core Changes

| EF6 | EF Core |
|---|---|
| `System.Data.Entity` | `Microsoft.EntityFrameworkCore` |
| `DbModelBuilder` | `ModelBuilder` |
| `HasRequired()` / `WithMany()` | `HasOne()` / `WithMany()` |
| `WillCascadeOnDelete(false)` | `OnDelete(DeleteBehavior.Restrict)` |
| `DbEntityEntry<T>` | `EntityEntry<T>` |
| `Database.Log` | `DbContextOptionsBuilder.LogTo(...)` |
| `.Include("Category")` string | `.Include(p => p.Category)` strongly-typed |
| `.ToList()` sync | `.ToListAsync()` async |
| Auto-initialization | Explicit: `Database.Migrate()` or `EnsureCreated()` |
| EF6 migrations | Start fresh: `Add-Migration InitialCreate` |

> **Note:** EF Core migrations are incompatible with EF6 migrations. Apply all EF6 migrations to the database first, then scaffold an initial EF Core migration with empty `Up`/`Down` methods to establish the baseline.

---

## Security Fixes Required During Migration

The following patterns in the source are SQL injection vulnerabilities that **must** be fixed:

```csharp
// VULNERABLE — raw string concatenation in query
string query = "SELECT * FROM Products WHERE Name LIKE '%" + name + "%'";

// FIXED — use EF Core parameterized query
var results = await _context.Products
    .Where(p => p.Name.Contains(name))
    .ToListAsync();
```

All raw `SqlConnection` / `SqlCommand` blocks using string interpolation or concatenation must be replaced with EF Core LINQ queries or parameterized `FromSqlRaw` with explicit parameters.

---

## Migration Phases

1. **Assess** — Inventory Framework-specific patterns (`ApiController`, `System.Web`, `ConfigurationManager`, `SqlConnection` raw queries, EF6 usage)
2. **Project** — Replace `.csproj` with SDK-style; add EF Core + ASP.NET Core packages
3. **Startup** — Create `Program.cs`; move connection string to `appsettings.json`
4. **Data layer** — Migrate `DbContext` constructor and model configuration to EF Core
5. **Controllers** — Update base class, return types, routing attributes, inject `DbContext`, make methods async
6. **Security** — Replace all raw SQL with parameterized EF Core queries
7. **Validate** — Run `dotnet build`, execute EF Core migrations, verify API endpoints

---

## Reference Docs

- [Migrate from ASP.NET Framework to ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/migration/fx-to-core/)
- [Migrate Web API from ASP.NET to ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/migration/webapi)
- [Port from EF6 to EF Core](https://learn.microsoft.com/en-us/ef/efcore-and-ef6/porting/)
- [EF Core DbContext configuration](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)