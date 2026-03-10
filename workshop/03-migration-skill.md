# Exercise 2: Build a Migration Skill & Agent

In this exercise, you'll create a reusable **skill** that encodes .NET Framework → .NET 10 migration knowledge, then build an **agent** that uses this skill to guide the modernization of the PartsCatalogAPI.

## Objectives

- Create a reusable migration skill with .NET Framework → .NET 10 knowledge
- Build an agent that leverages the skill for migration guidance
- Understand when to use skills vs agents
- Test the agent's ability to guide the PartsCatalogAPI migration

## Part 1: Create the Migration Skill

### Step 1: Design Your Migration Knowledge Base

You'll create a skill that documents everything needed to migrate .NET Framework 4.8 applications to .NET 10. GitHub Copilot supports [creating skills](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-skills) that encapsulate reusable knowledge and domain expertise.

1. Create a new folder structure:
    ```
    .github/
    └── skills/
        └── dotnet-framework-migration/
            └── SKILL.md
    ```

2. Design your skill in the `SKILL.md` file to document key migration knowledge. Consider including the following documentation:
    - **Core Framework Changes**:
        - Web API base classes: `ApiController` → `ControllerBase`
        - Return types: `IHttpActionResult` → `ActionResult<T>`
        - Project files: Full `.csproj` → SDK-style
        - Configuration: `Web.config` → `appsettings.json`
        - Startup: `Global.asax` + `WebApiConfig` → `Program.cs`
        - Entity Framework: EF6 → EF Core (especially async patterns)

        **Example: Document a Controller Migration**
        ```csharp
        // .NET Framework 4.8
        public class ProductsController : ApiController
        {
            public IHttpActionResult GetProducts() 
                => Ok(db.Products.ToList());
        }
        
        // .NET 10
        public class ProductsController : ControllerBase
        {
            private readonly PartsCatalogContext _context;
            public ProductsController(PartsCatalogContext context) => _context = context;
            
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
                => await _context.Products.ToListAsync();
        }
        // Note: Constructor injection, async/await, ActionResult<T>
        ```

    - **Critical Breaking Changes** - The gotchas that cause runtime issues:
        - Middleware order matters: `UseAuthentication()` must come before `UseAuthorization()`
        - Connection strings move from `ConfigurationManager` to `IConfiguration`
        - Async is required (can't mix sync database calls)
        - Model binding needs explicit attributes like `[FromBody]`
        - Package replacements (e.g., `EntityFramework` → `Microsoft.EntityFrameworkCore.SqlServer`)

> [!TIP]
> Use tables, code snippets, or any format that works for your team! The examples above are starting points - experiment with what best captures your migration knowledge. You can link to [official migration docs](https://learn.microsoft.com/en-us/aspnet/core/migration/) for deep dives.

> [!NOTE]
> Keep it focused on patterns and decisions specific to your migration. Add phase-by-phase strategies, package mappings, or architectural notes as needed for your project.

3. Save your skill definition

## Part 2: Create the Modernization Agent

### Step 2: Design an Agent That Uses Your Skill

Now you'll create an agent that can consult your migration skill and guide developers through the modernization process.

1. Create a new file: `.github/agents/modernization-expert.agent.md`
2. Design your agent gent definition. Include guidance on:
    - Role and behavior (what kind of migration expert it should act like)
    - How it should use the migration skill for framework-specific decisions
    - Preferred workflow (assess, plan, guide, validate, enhance)
    - Code response style (show current state, propose changes, explain why, include validation steps)
    - Principles to preserve (maintain behavior, migrate incrementally, consider trade-offs, keep security in scope)
3. Save your agent definition

**Example Interaction Pattern** - Show the agent how to respond:
```
User: "Modernize ProductsController"

Agent should:
1. Analyze current controller (base class, return types, sync methods)
2. Identify Framework patterns that need updating
3. Provide modernized version with explanations
4. Note downstream impacts (repository layer, tests)
5. Suggest validation steps
```

### Step 3: Test Your Agent and Skill Together

Let's verify your agent can guide the PartsCatalogAPI migration:

1. Open Copilot Chat (`Ctrl+I`)
2. Use the agent dropdown to select your modernization agent
3. Ask the agent to analyze `ProductsController.cs` and provide migration guidance. Consider asking about:
    - What .NET Framework patterns exist in the current code
    - How to modernize the controller for .NET 10
    - What breaking changes need to be addressed
    - Which packages or dependencies need updates
    - How to validate the modernized code works correctly
4. **Evaluate the response:**
    - Does the agent reference the migration skill?
    - Does it provide specific code examples?
    - Does it explain breaking changes clearly?
    - Does it mention async/await requirements?
    - Does it note EF Core migration needs?

> [!TIP]
> Your agent can reference the security agent too. Consider adding a principle: "Consult the security agent for vulnerability fixes during migration."

### Step 4: Compare Agent Approaches

Switch to your **Security Agent** in the dropdown and ask it to review the ProductsController migration plan, flagging any security concerns that should be addressed during the modernization.

**Notice the difference:**
- **Modernization Agent**: Focuses on framework patterns, breaking changes, best practices
- **Security Agent**: Focuses on vulnerabilities, secure coding, authentication
- **Both together**: Comprehensive migration that's secure and modern!

> [!TIP]
> You can switch agents in the dropdown during the same conversation:
> ```
> Select the modernization agent and ask: provide the migration approach
> Switch to the security agent and ask: review for security issues
> ```

### Step 5: Create a Complete Migration Plan

1. With your modernization agent selected, ask it to create a step-by-step migration plan for PartsCatalogAPI.
    - Breakdown into migration phases
    - File-by-file changes required
    - Dependencies between changes
    - Security fixes to include
    - Testing checkpoints and validation strategy

2. Save the generated plan:
    - Save the response to `migration-plan.md`
    - Review and refine with your team's specific needs

## Understanding Skills vs Agents

### When to Create a Skill

Use skills when you have:
- **Reusable knowledge** that applies across multiple projects
- **Reference information** like API mappings, breaking changes
- **Best practices** that don't change per project
- **Domain expertise** multiple agents might need

### When to Create an Agent

Use agents when you need:
- **Specialized workflows** with specific processes
- **Role-based assistance** (security auditor, migration guide)
- **Contextual decision-making** that varies per situation
- **Personality/communication style** for different tasks

### Can Agents Work Together?

**Yes!** Agents can reference each other:

```
Select the modernization agent: Plan the ProductsController migration
Switch to the security agent: Review the plan for security issues
Switch back to the modernization agent: Update the plan based on security feedback
```

This creates a **multi-agent workflow** where specialized agents collaborate!

## Success Criteria

- [ ] Created migration skill in `.github/skills/dotnet-framework-migration/SKILL.md`
- [ ] Skill documents breaking changes, package mappings, migration phases
- [ ] Created modernization agent that references the skill
- [ ] Agent provides context-aware migration guidance
- [ ] Tested agent on PartsCatalogAPI controllers
- [ ] Generated complete migration plan
- [ ] Understand when to use skills vs agents
- [ ] See how agents can work together

## Troubleshooting

**Agent not using the skill?**
- Verify the skill folder name matches what your agent instructions reference
- Make sure the skill's SKILL.md file exists
- Verify the description of the skill matches the intent
- Try explicitly asking: "Consult your migration skill for breaking changes"

**Agent responses too generic?**
- Add more specific instructions in the agent definition
- Include example interactions showing desired behavior
- Make the agent's process clearer (step 1, step 2, etc.)

**Skill too detailed or overwhelming?**
- Focus on key patterns and decisions, not exhaustive documentation
- Use tables for quick reference (package mappings)
- Link to official docs for deep dives

**Agents contradicting each other?**
- This is good! Different perspectives catch different issues
- Consolidate recommendations from both agents
- Update agent definitions to acknowledge each other

## Reflection Questions

1. What information belongs in a skill vs in an agent definition?
2. How does your modernization agent's guidance differ from generic Copilot?
3. When would you create additional agents that use the same migration skill?
4. How could you extend the migration skill for your company's specific patterns?
5. What other agent + skill combinations would be useful for your team?
