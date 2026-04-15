# Security Audit Report — PartsCatalogAPI

**Audit Date:** April 15, 2026  
**Target:** `src/PartsCatalogAPI` (.NET Framework 4.8, ASP.NET Web API 2)  
**Migration Target:** .NET 10 (ASP.NET Core)  
**Auditor:** GitHub Copilot Security Expert Agent  

---

## Executive Summary

The PartsCatalogAPI contains **4 Critical**, **5 High**, and **5 Medium** severity vulnerabilities. The most severe issues are two SQL injection flaws in the search endpoints, hardcoded credentials in `Web.config`, and a completely unauthenticated debug endpoint that exposes secrets at runtime. None of the API endpoints carry any authentication or authorization enforcement, making every write operation (POST, PUT, DELETE) publicly writable.

**All Critical and High findings must be remediated before migration to .NET 10.** Several of these issues (hardcoded secrets, SQL injection, missing auth) would be trivially exploitable in any environment where the API is reachable.

| Severity | Count |
|----------|-------|
| Critical | 4     |
| High     | 5     |
| Medium   | 5     |
| Low      | 2     |
| **Total**| **16**|

---

## Findings

---

### [CRITICAL-1] SQL Injection — Product Search

- **File:** `Controllers/ProductsController.cs`, line 44  
- **CWE:** CWE-89 (Improper Neutralization of Special Elements used in an SQL Command)  
- **OWASP:** A03:2021 – Injection  
- **Migration Impact:** Must fix **before** migration  

**Why it's a risk:** The `name` parameter is concatenated directly into a raw SQL string with no sanitization or parameterization. An attacker can inject arbitrary SQL to dump all table data, bypass filters, or drop objects.

**Vulnerable code:**
```csharp
// Controllers/ProductsController.cs — SearchProducts(string name)
string query = "SELECT * FROM Products WHERE Name LIKE '%" + name + "%'";
using (var command = new SqlCommand(query, connection))
```

**Proof of concept input:** `name='; DROP TABLE Products;--`

**Secure replacement (.NET 10 / EF Core):**
```csharp
// Using EF Core — no raw SQL needed at all
[HttpGet("search")]
public async Task<IActionResult> SearchProducts([FromQuery] string name)
{
    if (string.IsNullOrWhiteSpace(name))
        return BadRequest("name is required");

    var results = await _context.Products
        .Include(p => p.Category)
        .Where(p => p.Name.Contains(name))
        .ToListAsync();

    return Ok(results);
}

// If raw SQL is required, use parameterized queries:
var results = await _context.Products
    .FromSqlRaw("SELECT * FROM Products WHERE Name LIKE {0}", $"%{name}%")
    .ToListAsync();
```

---

### [CRITICAL-2] SQL Injection — Category Lookup by Name

- **File:** `Controllers/CategoriesController.cs`, line 44  
- **CWE:** CWE-89  
- **OWASP:** A03:2021 – Injection  
- **Migration Impact:** Must fix **before** migration  

**Why it's a risk:** Identical pattern to CRITICAL-1. The `name` parameter is concatenated into a SQL equality predicate. An attacker supplying `' OR '1'='1` returns all categories regardless of the filter.

**Vulnerable code:**
```csharp
// Controllers/CategoriesController.cs — GetCategoryByName(string name)
string query = "SELECT * FROM Categories WHERE Name = '" + name + "'";
using (var command = new SqlCommand(query, connection))
```

**Secure replacement (.NET 10 / EF Core):**
```csharp
[HttpGet("by-name")]
public async Task<IActionResult> GetCategoryByName([FromQuery] string name)
{
    if (string.IsNullOrWhiteSpace(name))
        return BadRequest("name is required");

    var category = await _context.Categories
        .FirstOrDefaultAsync(c => c.Name == name);

    return category is null ? NotFound() : Ok(category);
}
```

---

### [CRITICAL-3] Hardcoded Credentials and API Keys in Web.config

- **File:** `Web.config`, lines 5–9 and 12  
- **CWE:** CWE-798 (Use of Hard-coded Credentials)  
- **OWASP:** A07:2021 – Identification and Authentication Failures  
- **Migration Impact:** Must fix **before** migration  

**Why it's a risk:** Credentials embedded in source-controlled configuration files are exposed to every developer, CI system, and anyone with repository read access. The SA (`sa`) account password in the connection string grants full SQL Server administrative access.

**Vulnerable configuration:**
```xml
<appSettings>
  <add key="AdminUsername" value="admin" />
  <add key="AdminPassword" value="Password123!" />
  <add key="ApiKey" value="12345-ABCDE-67890-FGHIJ" />
  <add key="EnableDebugMode" value="true" />
</appSettings>
<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Server=(localdb)\mssqllocaldb;Database=PartsCatalog;User Id=sa;Password=YourPassword123;..."
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Secure replacement (.NET 10):**
```json
// appsettings.json — no secrets
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=PartsCatalog;Integrated Security=True;"
  }
}
```
```csharp
// Program.cs — load secrets from environment or Azure Key Vault
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddAzureKeyVault(new Uri(kvUri), new DefaultAzureCredential());
```

Remove all credential keys from `appsettings.json`. Use a dedicated low-privilege SQL login (never `sa`). Rotate the API key immediately — treat the current value as compromised.

---

### [CRITICAL-4] Unauthenticated Debug Endpoint Leaking Secrets

- **File:** `Controllers/ProductsController.cs`, lines 155–167  
- **CWE:** CWE-200 (Exposure of Sensitive Information to an Unauthorized Actor)  
- **OWASP:** A02:2021 – Cryptographic Failures / Information Disclosure  
- **Migration Impact:** Must fix **before** migration — remove the endpoint entirely  

**Why it's a risk:** `GET /api/Products/Debug` is publicly accessible with no authentication guard. It returns the `AdminUsername`, `ApiKey`, database server name, and database name in its JSON response. This single request yields everything an attacker needs to authenticate or pivot.

**Vulnerable code:**
```csharp
[HttpGet]
[Route("api/Products/Debug")]
public IHttpActionResult GetDebugInfo()
{
    var info = new
    {
        AdminUser = ConfigurationManager.AppSettings["AdminUsername"],
        ApiKey = ConfigurationManager.AppSettings["ApiKey"],
        ServerName = db.Database.Connection.DataSource,
        DatabaseName = db.Database.Connection.Database,
        // ...
    };
    return Ok(info);
}
```

**Remediation:** Delete this endpoint. It has no legitimate production purpose. Diagnostics belong in structured logging (Serilog/OpenTelemetry) and monitoring dashboards, never in an unauthenticated HTTP response.

---

### [HIGH-1] No Authentication or Authorization on Any Endpoint

- **Files:** `Controllers/ProductsController.cs`, `Controllers/CategoriesController.cs`  
- **CWE:** CWE-862 (Missing Authorization)  
- **OWASP:** A01:2021 – Broken Access Control  
- **Migration Impact:** Must fix **before** migration  

**Why it's a risk:** Every endpoint — including POST, PUT, DELETE, and BulkUpdate — is accessible without any credential. Any internet-connected client can create, modify, or delete all products and categories.

**Vulnerable code:**
```csharp
// No [Authorize] attribute anywhere
public class ProductsController : ApiController
{
    public IHttpActionResult DeleteProduct(int id) { ... }
    public IHttpActionResult PostProduct(Product product) { ... }
}
```

**Secure replacement (.NET 10):**
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]                           // Require auth on all actions by default
public class ProductsController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]                  // Explicitly opt read-only endpoints out
    public async Task<IActionResult> GetProducts() { ... }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]      // Restrict destructive actions further
    public async Task<IActionResult> DeleteProduct(int id) { ... }
}
```

Add JWT Bearer or API-key middleware in `Program.cs`:
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* configure */ });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
```

---

### [HIGH-2] Debug Mode and Full Error Disclosure Enabled

- **File:** `Web.config`, lines 17–18  
- **CWE:** CWE-94 / CWE-200  
- **OWASP:** A05:2021 – Security Misconfiguration  
- **Migration Impact:** Must fix **before** migration  

**Why it's a risk:** `compilation debug="true"` disables JIT optimizations and enlarges the attack surface. `customErrors mode="Off"` returns full .NET stack traces and connection string fragments to clients on any unhandled exception, aiding reconnaissance.

**Vulnerable configuration:**
```xml
<compilation debug="true" targetFramework="4.8" />
<customErrors mode="Off" />
```

Note: `Web.Release.config` correctly sets `customErrors mode="On"`, but the base `Web.config` is the active file in most CI/CD pipelines unless a transform is explicitly applied. `EnableDebugMode = true` in `appSettings` is also left active.

**Secure replacement (.NET 10):**
```csharp
// Program.cs
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}
```
```json
// appsettings.Production.json
{
  "Logging": { "LogLevel": { "Default": "Warning" } }
}
```

---

### [HIGH-3] SQL Server SA Account in Connection String

- **File:** `Web.config`, line 12  
- **CWE:** CWE-250 (Execution with Unnecessary Privileges)  
- **OWASP:** A04:2021 – Insecure Design  
- **Migration Impact:** Must fix **before** migration  

**Why it's a risk:** The `sa` account is the SQL Server built-in system administrator. Using it as the application login grants the application — and any attacker who compromises it — full server-level control including access to all databases, ability to create sysadmin logins, and `xp_cmdshell` execution.

**Remediation:** Create a dedicated, least-privilege SQL login:
```sql
CREATE LOGIN PartsCatalogApp WITH PASSWORD = '<strong-generated-password>';
CREATE USER PartsCatalogApp FOR LOGIN PartsCatalogApp;
-- Grant only what is needed
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO PartsCatalogApp;
```

Prefer Managed Identity (passwordless) when deploying to Azure:
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<server>.database.windows.net;Database=PartsCatalog;Authentication=Active Directory Default;"
  }
}
```

---

### [HIGH-4] DropCreateDatabaseIfModelChanges Initializer Active

- **File:** `Data/PartsCatalogInitializer.cs`, line 9  
- **CWE:** CWE-693 (Protection Mechanism Failure)  
- **OWASP:** A05:2021 – Security Misconfiguration  
- **Migration Impact:** Must fix **before** migration  

**Why it's a risk:** `DropCreateDatabaseIfModelChanges<T>` will silently drop and recreate the entire database whenever the EF model changes. In any environment where this code reaches a non-development database, a model change causes catastrophic, irreversible data loss.

**Vulnerable code:**
```csharp
public class PartsCatalogInitializer : DropCreateDatabaseIfModelChanges<PartsCatalogContext>
```

**Secure replacement (.NET 10 / EF Core):**
```csharp
// Program.cs — apply pending migrations at startup, never drop
await using var scope = app.Services.CreateAsyncScope();
var db = scope.ServiceProvider.GetRequiredService<PartsCatalogContext>();
await db.Database.MigrateAsync();
```

Seed data should be idempotent (use `HasData` in `OnModelCreating`) and never tied to destructive initializers.

---

### [HIGH-5] Newtonsoft.Json 9.0.1 — DoS Vulnerability

- **File:** `packages.config`, line 6  
- **CVE:** CVE-2024-21907  
- **CVSS:** 7.5 (High)  
- **Affected versions:** < 13.0.2  
- **Fixed version:** 13.0.2+  
- **Migration Impact:** Must fix **during** migration  

**Why it's a risk:** Newtonsoft.Json versions before 13.0.2 are vulnerable to a stack overflow and denial-of-service condition when deserializing deeply or cyclically nested JSON objects. An attacker can crash the application process by sending a crafted request body.

**Remediation:** In .NET 10 migration, replace `Newtonsoft.Json` with `System.Text.Json` (built-in, no CVEs):
```csharp
// Program.cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
```

If Newtonsoft.Json must be retained, upgrade to **13.0.3** minimum:
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

---

### [MEDIUM-1] EntityFramework 6.1.3 — Outdated, Unsupported Version

- **File:** `packages.config`, line 2  
- **CVE:** Multiple security improvements in 6.2.0–6.4.4 (no single CVE, but 6.1.3 predates important fixes for SQL generation edge cases)  
- **Current stable EF6:** 6.4.4  
- **Migration Target:** EF Core 10  
- **Migration Impact:** Must fix **during** migration  

**Why it's a risk:** EF 6.1.3 is significantly behind the current EF6 line. EF 6.4.4 includes fixes for SQL generation issues and security hardening. More critically, EF6 is not supported on .NET 10 — migration requires moving to EF Core 10.

**Remediation during migration:**
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0" />
```

---

### [MEDIUM-2] Microsoft.AspNet.WebApi 5.2.3 — End-of-Life Framework

- **File:** `packages.config`, lines 3–5  
- **Migration Impact:** Must fix **during** migration (this is the framework being replaced)  

**Why it's a risk:** ASP.NET Web API 2 on .NET Framework 4.8 reached end of mainstream support. It does not receive security patches. The entire stack must be replaced with ASP.NET Core on .NET 10.

**Remediation:** Replace with `Microsoft.AspNetCore.App` framework reference. All `ApiController`, `HttpConfiguration`, `IHttpActionResult` patterns must be converted to `ControllerBase`, `IActionResult`, and minimal API equivalents.

---

### [MEDIUM-3] Missing Security Response Headers

- **File:** `App_Start/WebApiConfig.cs`  
- **CWE:** CWE-693  
- **OWASP:** A05:2021 – Security Misconfiguration  
- **Migration Impact:** Fix **during** migration  

**Why it's a risk:** No HTTP security headers are configured. Clients are not protected against clickjacking (`X-Frame-Options`), MIME-sniffing (`X-Content-Type-Options`), or downgrade attacks (HSTS).

**Secure replacement (.NET 10):**
```csharp
// Program.cs
app.UseHsts();
app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});
```

---

### [MEDIUM-4] CORS Not Configured

- **File:** `App_Start/WebApiConfig.cs`  
- **CWE:** CWE-346 (Origin Validation Error)  
- **OWASP:** A05:2021 – Security Misconfiguration  
- **Migration Impact:** Fix **during** migration  

**Why it's a risk:** No CORS policy is defined. The browser default varies by host environment. Without explicit CORS configuration, either all origins are implicitly trusted or legitimate browser clients are blocked. Neither outcome is correct.

**Secure replacement (.NET 10):**
```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTrustedOrigins", policy =>
        policy.WithOrigins("https://your-frontend.example.com")
              .AllowedMethods("GET", "POST", "PUT", "DELETE")
              .AllowCredentials());
});

app.UseCors("AllowTrustedOrigins");
```

---

### [MEDIUM-5] No Input Validation on ImageUrl Fields

- **Files:** `Models/Product.cs` line 31, `Models/Category.cs` line 18  
- **CWE:** CWE-20 (Improper Input Validation)  
- **OWASP:** A03:2021 – Injection  
- **Migration Impact:** Fix **during** migration  

**Why it's a risk:** `ImageUrl` accepts any string up to 500/200 characters including `javascript:` URIs, `data:` URIs, and path traversal sequences. If any part of the application renders these URLs in an HTML context without escaping, this becomes a stored XSS vector.

**Vulnerable model:**
```csharp
[StringLength(500)]
public string ImageUrl { get; set; }
```

**Secure replacement (.NET 10):**
```csharp
[StringLength(500)]
[RegularExpression(@"^https?://[^\s<>""]+$", ErrorMessage = "ImageUrl must be a valid http/https URL")]
public string? ImageUrl { get; set; }
```

Or use a custom validator that calls `Uri.TryCreate` and enforces `https` scheme.

---

### [LOW-1] DbContext Instantiated as Controller Field

- **Files:** `Controllers/ProductsController.cs` line 14, `Controllers/CategoriesController.cs` line 14  
- **CWE:** CWE-404 (Improper Resource Shutdown or Release)  
- **Migration Impact:** Fix **during** migration  

**Why it's a risk:** `private PartsCatalogContext db = new PartsCatalogContext()` creates the DbContext as a field. In EF Core, DbContext is not thread-safe. Using constructor injection with a scoped lifetime is the correct pattern.

**Secure replacement (.NET 10):**
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly PartsCatalogContext _context;

    public ProductsController(PartsCatalogContext context)
    {
        _context = context;
    }
}
```
```csharp
// Program.cs
builder.Services.AddDbContext<PartsCatalogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

### [LOW-2] DateTime.Now Instead of DateTime.UtcNow

- **Files:** `Controllers/ProductsController.cs` lines 78, 115; `Data/PartsCatalogInitializer.cs`  
- **CWE:** CWE-704  
- **Migration Impact:** Fix **during** migration  

**Why it's a risk:** `DateTime.Now` produces server-local time. In cloud deployments where the server timezone may differ from the application's logical timezone, timestamps become inconsistent and cannot be reliably compared across instances.

**Remediation:** Replace all `DateTime.Now` with `DateTime.UtcNow`, and consider using `DateTimeOffset.UtcNow` for unambiguous timestamp storage.

---

## Package Vulnerability Summary

| Package | Current Version | Fixed Version | CVE | Severity |
|---------|----------------|---------------|-----|----------|
| Newtonsoft.Json | 9.0.1 | 13.0.3 | CVE-2024-21907 | High |
| EntityFramework | 6.1.3 | 6.4.4 (or EF Core 10) | Multiple security fixes in 6.2–6.4 | Medium |
| Microsoft.AspNet.WebApi | 5.2.3 | N/A — replace with ASP.NET Core 10 | End-of-life | High |
| Microsoft.AspNet.WebApi.Client | 5.2.3 | N/A — replace with HttpClient/Refit | End-of-life | High |

---

## Remediation Roadmap

### Phase 1 — Fix Before Migration (Immediate)

These issues exist in the running .NET Framework application today and must be resolved regardless of migration status.

| # | Finding | Action |
|---|---------|--------|
| CRITICAL-1 | SQL Injection (Products) | Replace raw SQL with EF parameterized queries |
| CRITICAL-2 | SQL Injection (Categories) | Replace raw SQL with EF parameterized queries |
| CRITICAL-3 | Hardcoded credentials | Remove from Web.config; move to environment variables or Key Vault |
| CRITICAL-4 | Debug endpoint | Delete `GetDebugInfo()` action entirely |
| HIGH-1 | No authentication | Add ASP.NET Web API `[Authorize]` filter globally |
| HIGH-2 | Debug mode / CustomErrors Off | Set `debug="false"` and `customErrors mode="RemoteOnly"` in Web.config |
| HIGH-3 | SA account | Create least-privilege SQL login; rotate all passwords |
| HIGH-4 | DropCreate initializer | Remove from production; use explicit migrations |

### Phase 2 — Fix During Migration

Address while porting the codebase to .NET 10 / ASP.NET Core.

| # | Finding | Action |
|---|---------|--------|
| HIGH-5 | Newtonsoft.Json CVE | Replace with System.Text.Json |
| MEDIUM-1 | EntityFramework 6.1.3 | Migrate to EF Core 10 |
| MEDIUM-2 | ASP.NET Web API 5.2.3 | Replace with ASP.NET Core controllers |
| MEDIUM-3 | Missing security headers | Add middleware in Program.cs |
| MEDIUM-4 | CORS not configured | Define explicit CORS policy |
| MEDIUM-5 | ImageUrl validation | Add URL format validation attribute |
| LOW-1 | DbContext field | Use constructor injection with scoped lifetime |
| LOW-2 | DateTime.Now | Replace with DateTime.UtcNow |

### Phase 3 — Post-Migration Hardening

Recommended after the .NET 10 baseline is running in staging.

- Enable rate limiting middleware (`app.UseRateLimiter()`)
- Add OpenTelemetry tracing to all database calls
- Implement structured logging with Serilog (no sensitive data in log sinks)
- Add integration tests for authorization boundaries
- Enable Azure Defender for SQL for continuous vulnerability assessment
- Set up Dependabot or Renovate for automated package CVE alerts

---

*Report generated by GitHub Copilot Security Expert Agent. All line numbers reference the original .NET Framework 4.8 source tree.*
