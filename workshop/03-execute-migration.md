# Exercise 3: Execute the Migration

**Duration:** 35 minutes

Now it's time to put your custom agents to work! Using the security audit and migration plan you created, you'll modernize the Product Cat API from .NET Framework 4.8 to .NET 10.

## Objectives

- Use your custom agents to guide actual code changes
- Execute a phased migration approach
- Fix security vulnerabilities during modernization
- Validate that the modernized app works

## Migration Approach

We'll use an **incremental strategy**:
1. **Update project structure** - Convert to SDK-style project
2. **Modernize dependencies** - Update packages
3. **Fix code patterns** - Update controllers, services, data access
4. **Update configuration** - Move to appsettings.json
5. **Validate & test** - Ensure everything works

> [!NOTE]
> Your agents will maintain context across all these steps. They "remember" what you've changed and suggest consistent patterns.

## Part 1: Project Structure Modernization (10 minutes)

### Step 1: Convert Project File

Ask your modernization agent:

```
@modernization-expert

Convert the ProductCatalogAPI.csproj from old-style to SDK-style project file.

Current state: .NET Framework 4.8 with packages.config
Target: .NET 10 SDK-style with PackageReference

Generate the new .csproj file contents.
```

**Expected agent response should include:**
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="7.0.0" />
  </ItemGroup>
</Project>
```

Apply the changes to your project file.

### Step 2: Create Program.cs

Ask your agent:

```
@modernization-expert

Create a Program.cs file for .NET 10 that:
1. Configures services (DbContext, DI)
2. Sets up middleware (auth, HTTPS, CORS)
3. Configures Swagger/OpenAPI
4. Loads configuration from appsettings.json

Use the minimal hosting model.
```

Create the new `Program.cs` file with the agent's response.

### Step 3: Remove Legacy Files

Delete files that are no longer needed:
- `Web.config` (will become `appsettings.json`)
- `Global.asax`
- `App_Start/WebApiConfig.cs`
- `packages.config`

> [!TIP]
> Don't delete until you've migrated the configuration values!

## Part 2: Code Modernization (15 minutes)

### Step 4: Modernize Controllers

Work with both agents together!

```
@modernization-expert @security-modernization

Modernize the ProductsController:

Modernization requirements:
- Change base class to ControllerBase
- Update return types to IActionResult
- Make all methods async
- Update routing attributes
- Implement proper DI

Security requirements:
- Fix the SQL injection vulnerability in Search()
- Add [Authorize] attributes
- Add input validation
- Use parameterized queries via EF Core

Provide the complete modernized controller.
```

**The agents working together should:**
- Modernization agent: Handles framework patterns
- Security agent: Ensures secure coding practices
- Both: Consistent with previous recommendations

### Step 5: Update Data Access Layer

Ask your modernization agent:

```
@modernization-expert

Migrate the Entity Framework 6 DbContext to EF Core 9:

1. Update the DbContext class
2. Make all methods async
3. Update connection string handling
4. Add proper configuration

Show before/after for the ProductContext class.
```

Apply the changes.

### Step 6: Update Models & DTOs

Your agent can help with model updates:

```
@modernization-expert

Review all model classes and update them for .NET 10 best practices:
- Enable nullable reference types
- Add data annotations for validation
- Update any .NET Framework-specific attributes
```

## Part 3: Configuration Migration (5 minutes)

### Step 7: Create appsettings.json

Extract configuration from Web.config:

```
@modernization-expert

Extract all configuration from Web.config and create:
1. appsettings.json (base config)
2. appsettings.Development.json (dev-specific)

Include:
- Connection strings
- Logging configuration
- CORS settings
- JWT auth settings
```

Create the appsettings files with the agent's generated content.

> [!WARNING]
> Don't commit actual connection strings! Use environment variables or Azure Key Vault for production.

### Step 8: Update Authentication

If your app has authentication:

```
@security-modernization

Implement JWT Bearer authentication in Program.cs that:
1. Validates JWT tokens
2. Reads configuration from appsettings.json
3. Uses modern authentication middleware
4. Follows security best practices
```

## Part 4: Testing & Validation (5 minutes)

### Step 9: Build the Application

```bash
dotnet build
```

If you get errors, ask your agent:

```
@modernization-expert

I'm getting this build error:
[paste error]

What's the cause and how do I fix it?
```

### Step 10: Run the Application

```bash
dotnet run
```

The API should start on `https://localhost:5001`

### Step 11: Test Endpoints

Navigate to `https://localhost:5001/swagger`

Test:
- GET /api/products - Should return product list
- GET /api/products/{id} - Should return single product
- POST /api/products - Should create product (requires auth)
- PUT /api/products/{id} - Should update product (requires auth)
- DELETE /api/products/{id} - Should delete product (requires auth)

### Step 12: Re-run Security Audit

Compare before and after:

```
@security-modernization

Perform a new security audit of the modernized application.

Compare findings to the original audit in docs/security-audit.md.

Generate a report showing:
1. Issues resolved
2. Improvements made
3. Any remaining concerns
4. Security score before/after

Save as docs/security-audit-after-migration.md
```

Review the improvements!

## Part 5: Modern Enhancements (Optional, if time permits)

### Step 13: Add Health Checks

```
@modernization-expert

Add health check endpoints:
1. Basic health check at /health
2. Database connectivity check
3. Configure in Program.cs
```

### Step 14: Add Structured Logging

```
@modernization-expert

Configure Serilog for structured logging:
1. Log to console and file
2. Include request/response logging
3. Add correlation IDs
```

## Success Criteria

- [ ] Project converted to SDK-style targeting .NET 10
- [ ] All controllers modernized (async, ControllerBase, IActionResult)
- [ ] EF6 migrated to EF Core 9
- [ ] Configuration moved to appsettings.json
- [ ] Security vulnerabilities fixed
- [ ] Application builds successfully
- [ ] Application runs and serves requests
- [ ] Swagger/OpenAPI documentation works
- [ ] All tests pass (if you have tests)
- [ ] Security audit shows improvement

## Troubleshooting

**Build errors about missing packages?**
```bash
dotnet restore
```

**Runtime errors about configuration?**
- Check appsettings.json syntax
- Verify connection string format
- Ensure IConfiguration is injected properly

**Database errors?**
- Update connection string to EF Core format
- Run migrations: `dotnet ef database update`
- Check SQL Server is running

**Authentication not working?**
- Verify JWT configuration in appsettings.json
- Check middleware order (UseAuthentication before UseAuthorization)
- Test with valid JWT token

## Commit Your Work

```bash
git add .
git commit -m "Migrate ProductCatalogAPI from .NET Framework 4.8 to .NET 10

- Converted to SDK-style project
- Modernized controllers with async/await
- Migrated EF6 to EF Core 9
- Fixed security vulnerabilities
- Added modern authentication
- Configured health checks and logging"

git push origin main
```

## Compare: With vs Without Custom Agents

### Without Custom Agents
- ⏱️ **Time**: 3-5 days of research and trial/error
- 🔍 **Context Loss**: Forget what you changed, repeat searches
- ❌ **Inconsistent Patterns**: Different approaches in different files
- 🐛 **Missed Issues**: Security vulnerabilities slip through
- 📚 **Constant Research**: Reading docs for every decision

### With Custom Agents
- ⏱️ **Time**: 2-3 hours of guided migration
- 🧠 **Context Retained**: Agent remembers all changes
- ✅ **Consistent Patterns**: Same approach throughout
- 🛡️ **Security Validated**: Security agent catches issues
- 🎯 **Focused Work**: Agent handles research, you handle decisions

## Reflection Questions

1. How many times did your agents reference previous context?
2. What would have taken longest without agent guidance?
3. What migration patterns would you add to your skill?
4. How would you use these agents on your real projects?

---

**Congratulations! You've completed the migration!** Proceed to [Review & Next Steps](./04-review.md).
