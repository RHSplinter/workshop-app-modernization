# Exercise 2: Build a Modernization Skill & Agent

**Duration:** 30 minutes

In this exercise, you'll create a reusable **skill** that encodes .NET Framework → .NET 10 migration best practices, then build an **agent** that uses this skill to guide the modernization process.

## Skills vs Agents: What's the Difference?

### Skills
- **Reusable knowledge modules** that can be used by multiple agents
- Encode domain expertise, patterns, and best practices
- Like a "textbook" or "reference guide" for specific topics
- Example: "Framework Migration Patterns", "API Testing Strategies"

### Agents
- **AI assistants with specific roles** that use skills
- Have personality, process, and decision-making logic
- Like a "senior engineer" with specialized knowledge
- Example: "Modernization Agent" that uses the migration skill

> [!NOTE]
> Think of skills as **what you know** and agents as **how you work**.

## Objectives

- Create a reusable modernization skill
- Build an agent that leverages the skill
- Understand when to use skills vs agents
- Test the agent's ability to guide migration

## Part 1: Create the Modernization Skill (15 minutes)

### Step 1: Create the Skill File

1. Create a new folder structure:
   ```
   .github/
   └── skills/
       └── dotnet-framework-modernization/
           └── SKILL.md
   ```

2. Open `SKILL.md` and add the following:

```markdown
---
name: .NET Framework to .NET 10 Migration
description: Best practices, patterns, and breaking changes for migrating .NET Framework applications to modern .NET
tags:
  - dotnet
  - migration
  - modernization
  - netframework
  - net10
---

# .NET Framework to .NET 10 Migration Skill

This skill provides comprehensive guidance for migrating .NET Framework 4.x applications to .NET 10.

## Migration Overview

### What Changes

| .NET Framework 4.8 | .NET 10 |
|--------------------|---------|
| `System.Web` | `Microsoft.AspNetCore` |
| `packages.config` | SDK-style `<PackageReference>` |
| `Web.config` | `appsettings.json` |
| `Global.asax` | `Program.cs` + `Startup.cs` (or minimal APIs) |
| `WebApiConfig` | Middleware configuration |
| EF6 | EF Core 9+ |
| Synchronous APIs | Async/await patterns |

### Breaking Changes

#### 1. System.Web Dependencies
**Old:** `System.Web.Http.ApiController`  
**New:** `Microsoft.AspNetCore.Mvc.ControllerBase`

**Old:** `[RoutePrefix("api/products")]`  
**New:** `[Route("api/[controller]")]`

**Old:** `IHttpActionResult`  
**New:** `IActionResult`

#### 2. Dependency Injection
**Old:** Unity, Ninject (manual setup)  
**New:** Built-in DI container

```csharp
// Register in Program.cs
builder.Services.AddScoped<IProductRepository, ProductRepository>();
```

#### 3. Configuration
**Old:** `ConfigurationManager.AppSettings["key"]`  
**New:** `IConfiguration` injection

```csharp
public class MyController : ControllerBase
{
    private readonly IConfiguration _config;
    
    public MyController(IConfiguration config)
    {
        _config = config;
    }
}
```

#### 4. Authentication
**Old:** `Web.config` + `[Authorize]` attribute  
**New:** Middleware + modern auth schemes

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* config */ });

app.UseAuthentication();
app.UseAuthorization();
```

#### 5. Database Access
**Old:** Entity Framework 6 (DbContext)  
**New:** Entity Framework Core 9

**Key differences:**
- Async methods become primary
- No lazy loading by default
- Different migration commands
- Connection string in `appsettings.json`

```csharp
// Old EF6
public Product GetProduct(int id)
{
    return db.Products.Find(id);
}

// New EF Core
public async Task<Product?> GetProductAsync(int id)
{
    return await db.Products.FindAsync(id);
}
```

## Migration Strategy

### Phase 1: Project File Conversion
1. Convert to SDK-style project file
2. Update target framework: `<TargetFramework>net10.0</TargetFramework>`
3. Convert `packages.config` to `PackageReference`
4. Remove unnecessary references (System.Web, etc.)

### Phase 2: Update Dependencies
1. Replace `Microsoft.AspNet.WebApi` with `Microsoft.AspNetCore.App`
2. Update EF6 to EF Core 9
3. Update all NuGet packages to .NET 10 compatible versions
4. Remove packages that are now part of the framework

### Phase 3: Code Modernization
1. Replace `ApiController` with `ControllerBase`
2. Update return types (`IHttpActionResult` → `IActionResult`)
3. Add async/await to all I/O operations
4. Replace `Web.config` settings with `appsettings.json`
5. Update routing attributes
6. Implement proper DI patterns

### Phase 4: Configuration & Middleware
1. Create `Program.cs` with service registration
2. Configure middleware pipeline (auth, CORS, etc.)
3. Migrate connection strings
4. Configure logging and health checks

### Phase 5: Testing & Validation
1. Ensure all endpoints work
2. Verify authentication/authorization
3. Run security scan again
4. Performance testing

## Common Pitfalls

1. **Forgetting to make methods async** - All I/O should be async
2. **Not updating connection strings** - They move from Web.config to appsettings.json
3. **Missing middleware order** - Auth must come after routing
4. **Breaking change in model binding** - FromBody is explicit in .NET Core
5. **Missing nullable reference types** - Consider enabling for better safety

## Package Mapping

| .NET Framework Package | .NET 10 Replacement |
|------------------------|---------------------|
| `Microsoft.AspNet.WebApi.Core` | `Microsoft.AspNetCore.App` |
| `EntityFramework` | `Microsoft.EntityFrameworkCore` |
| `Newtonsoft.Json` | `System.Text.Json` (or keep Newtonsoft) |
| `Unity` / `Ninject` | Built-in DI |
| `Swashbuckle` (WebAPI) | `Swashbuckle.AspNetCore` |

## Testing Checklist

After migration, verify:

- [ ] All HTTP verbs (GET, POST, PUT, DELETE) work
- [ ] Authentication tokens are validated
- [ ] Database queries execute correctly
- [ ] Configuration loads from appsettings.json
- [ ] HTTPS redirection works
- [ ] CORS policy is configured (if needed)
- [ ] Health check endpoint responds
- [ ] Swagger/OpenAPI docs generate correctly

## Modern Enhancements

Consider adding:

1. **Minimal APIs** - Lighter weight for simple endpoints
2. **Health Checks** - `/health` endpoint for monitoring
3. **OpenTelemetry** - Distributed tracing
4. **Rate Limiting** - Built-in rate limiting middleware
5. **Output Caching** - New in .NET 7+
6. **Native AOT** - Faster startup and smaller deployments

## Security Improvements

- ✅ Enable HTTPS redirection
- ✅ Implement JWT authentication
- ✅ Use parameterized queries (EF Core does this automatically)
- ✅ Enable request validation
- ✅ Configure CORS properly
- ✅ Use secrets management (Azure Key Vault, User Secrets)

## Resources

- [Official .NET Upgrade Assistant](https://dotnet.microsoft.com/platform/upgrade-assistant)
- [Breaking Changes Documentation](https://learn.microsoft.com/en-us/dotnet/core/compatibility/)
- [ASP.NET Core Migration Guide](https://learn.microsoft.com/en-us/aspnet/core/migration/)
```

3. Save the file

> [!TIP]
> This skill is now reusable across ALL .NET Framework migration projects!

### Step 2: Verify Skill is Available

Skills don't appear as `@mentions`, but agents can reference them. Let's test:

1. Open Copilot Chat
2. Ask: "What skills are available for .NET migration?"
3. Copilot should recognize your skill in `.github/skills/`

## Part 2: Create the Modernization Agent (15 minutes)

### Step 3: Build the Agent That Uses the Skill

1. Create `.github/agents/modernization-expert.agent.md`:

```markdown
---
name: .NET Modernization Expert
description: Guides safe migration from .NET Framework to modern .NET with context-aware recommendations
expertise:
  - .NET Framework to .NET 10 migration
  - Breaking changes and compatibility
  - Async/await patterns
  - Modern ASP.NET Core patterns
skills:
  - dotnet-framework-modernization
---

# .NET Modernization Expert

You are a senior .NET engineer specializing in migrating legacy .NET Framework applications to modern .NET. You understand the full migration path from project files to runtime behavior.

## Your Skills

You have access to the `dotnet-framework-modernization` skill that contains:
- Breaking changes reference
- Migration patterns
- Package mappings
- Testing checklists

Refer to this skill when answering migration questions.

## Your Role

1. **Assess** - Analyze the current codebase structure
2. **Plan** - Create a step-by-step migration plan
3. **Guide** - Walk through each change with explanations
4. **Validate** - Verify changes maintain functionality
5. **Enhance** - Suggest modern improvements

## Your Process

When asked to modernize code:

1. **Show Current State** - Explain what the code currently does
2. **Identify Issues** - Point out Framework-specific patterns
3. **Provide Modern Version** - Show the .NET 10 equivalent
4. **Explain Changes** - Describe WHY each change is needed
5. **Test Guidance** - Suggest how to verify the change works

## Important Principles

- **Maintain Functionality** - Changes should preserve behavior
- **Incremental Migration** - One component at a time
- **Explain Trade-offs** - Modern isn't always "better" - explain when
- **Context Awareness** - Remember changes across files
- **Security First** - Fix security issues during migration

## Example Interaction

User: "Modernize ProductsController"

You should:
1. Analyze the current controller code
2. Identify .NET Framework patterns (IHttpActionResult, sync methods)
3. Provide modernized version with ControllerBase, async/await
4. Explain each change (why IActionResult, why async)
5. Note any dependencies that need updating (repository layer)
6. Suggest testing approach

## Communication Style

- **Be specific** - Reference exact files and line numbers
- **Show code** - Always provide before/after examples
- **Explain impact** - Describe downstream effects
- **Stay pragmatic** - Focus on what matters for this migration
- **Remember context** - Track what you've already updated

## You Don't Just Modernize - You Teach

Help developers understand:
- Why .NET 10 is structured differently
- What modern patterns solve
- When to use new features vs keep old patterns
- How changes affect performance, security, maintainability
```

2. Save the file

### Step 4: Test Your Modernization Agent

Let's see if your agent can guide migration:

1. Open Copilot Chat
2. Test with this prompt:

```
@modernization-expert 

I need to migrate the ProductsController from .NET Framework 4.8 to .NET 10.

Current controller:
- Uses ApiController base class
- Returns IHttpActionResult
- Has synchronous database calls
- Uses string concatenation for SQL (security issue!)

Please:
1. Analyze the current code
2. Provide the modernized version
3. Explain each major change
4. Note any dependencies I'll need to update
5. Suggest how to test the changes
```

3. Review the response. Your agent should:
   - Reference the migration skill
   - Provide specific code examples
   - Explain breaking changes
   - Note async/await requirements
   - Mention EF Core migration needs

### Step 5: Compare Agents

Now ask the same question to your **Security Agent**:

```
@security-modernization

Review this migration plan for ProductsController and flag any security concerns.
```

Notice how each agent approaches the same code differently:
- **Modernization Agent**: Focuses on patterns and framework changes
- **Security Agent**: Focuses on vulnerabilities and secure coding

> [!TIP]
> You can use BOTH agents together! One plans the migration, the other validates security.

### Step 6: Create a Migration Plan

Ask your modernization agent to create a complete plan:

```
@modernization-expert

Create a complete migration plan for the entire ProductCatalogAPI application.

Include:
1. Project file changes
2. Package updates (with version numbers)
3. Code changes (prioritized by component)
4. Configuration migration (Web.config → appsettings.json)
5. Testing strategy
6. Estimated effort per component

Save this as docs/migration-plan.md
```

### Step 7: Save and Commit

```bash
git add .github/skills/dotnet-framework-modernization/
git add .github/agents/modernization-expert.agent.md
git add docs/migration-plan.md
git commit -m "Add modernization skill and agent with migration plan"
```

## Understanding Skills vs Agents in Action

### The Skill (Knowledge)
- Contains facts about .NET migration
- Reusable across projects and agents
- Like a reference manual

### The Agent (Application)
- Uses the skill to make decisions
- Has personality and process
- Maintains context across conversation
- Like a consultant who read the manual

## Success Criteria

- [ ] Modernization skill created with migration patterns
- [ ] Modernization agent created that references the skill
- [ ] Agent provides specific, contextual migration guidance
- [ ] Agent explains WHY changes are needed, not just WHAT
- [ ] Migration plan generated and saved
- [ ] You understand the difference between skills and agents
- [ ] Changes committed to Git

## Troubleshooting

**Agent not using the skill?**
- Check the `skills:` array in agent frontmatter
- Skill name must match folder name
- Try asking "What skills do you have access to?"

**Agent responses too generic?**
- Add more specific patterns to the skill
- Improve agent instructions with examples
- Provide more context in your prompts

**Can't decide skill vs agent?**
- If it's reusable knowledge → Skill
- If it's a role/process → Agent
- Agents can use multiple skills!

## Reflection Questions

1. What migration patterns would you add to the skill based on your experience?
2. How could you create skills for other domains (testing, performance, architecture)?
3. What other agents could use this modernization skill?

---

**Ready to actually modernize the code?** Proceed to [Exercise 3: Execute the Migration](./03-execute-migration.md).
