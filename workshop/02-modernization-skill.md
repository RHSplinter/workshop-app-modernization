# Exercise 2: Build a Modernization Skill & Agent

In this exercise, you'll create a reusable **skill** that encodes .NET Framework → .NET 10 migration knowledge, then build an **agent** that uses this skill to guide the modernization of the PartsCatalogAPI.

## Objectives

- Create a reusable modernization skill with .NET Framework → .NET 10 knowledge
- Build an agent that leverages the skill for migration guidance
- Understand when to use skills vs agents
- Test the agent's ability to guide the PartsCatalogAPI migration

## Part 1: Create the Migration Skill

### Step 1: Design Your Migration Knowledge Base

You'll create a skill that documents everything needed to migrate .NET Framework 4.8 applications to .NET 10.

> Reference: [Create skills (GitHub Docs)](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-skills)

1. Create a new folder structure:
    ```
    .github/
    └── skills/
        └── dotnet-framework-migration/
            └── SKILL.md
    ```
<!-- TODO: Improve -->
2. Design your skill. Include documentation on:
    - Framework and API differences (System.Web → Microsoft.AspNetCore)
    - Project structure conversion (packages.config → PackageReference style projects)
    - Configuration migration strategies (Web.config → appsettings.json)
    - Startup pipeline changes (Global.asax/WebApiConfig → Program.cs)
    - Data access layer modernization (EF6 → EF Core)
    - Known breaking changes in controllers, routing, return types, ASP.NET Core DI, and auth
    - Phase-by-phase migration strategy
    - Package mapping reference (what replaces old packages)
    - Common pitfalls and how to avoid them
    - Before/after code patterns for typical migration scenarios

<!-- TODO: Improve -->
> [!TIP]
> Look at the PartsCatalogAPI structure to inform your skill:
> - [ProductsController.cs](../src/PartsCatalogAPI/Controllers/ProductsController.cs) uses `ApiController` and `IHttpActionResult`
> - [PartsCatalogContext.cs](../src/PartsCatalogAPI/Data/PartsCatalogContext.cs) uses Entity Framework 6 patterns
> - [Web.config](../src/PartsCatalogAPI/Web.config) has connection strings and app settings
> - [packages.config](../src/PartsCatalogAPI/packages.config) lists legacy packages

Also consider documenting a phase-by-phase migration strategy:
1. **Phase 1**: Project file conversion (SDK-style, target framework)
2. **Phase 2**: Dependencies (package updates, remove System.Web)
3. **Phase 3**: Code patterns (controllers, async/await, DI)
4. **Phase 4**: Configuration (appsettings.json, middleware)
5. **Phase 5**: Testing & validation

Also include package mapping guidance:
- What replaces `Microsoft.AspNet.WebApi.Core`?
- What's the EF Core equivalent of `EntityFramework` 6.1.3?
- Should `Newtonsoft.Json` 9.0.1 be updated or replaced with `System.Text.Json`?
- What happens to Unity/Ninject? (Built-in DI)

Also include common pitfalls:
- Forgetting async/await patterns
- Connection string location changes
- Middleware ordering (CRITICAL: UseAuthentication before UseAuthorization)
- Model binding differences (FromBody explicit in .NET Core)
- Nullable reference types considerations

Also include before/after examples for common patterns:
- Controller action migration
- Async database calls
- Configuration access
- Dependency injection setup


> [!NOTE]
> The skill should be comprehensive - it's your team's migration reference. But don't make it a full tutorial; focus on the patterns and decisions specific to .NET Framework → .NET 10.

3. Save your skill definition


## Part 2: Create the Modernization Agent

### Step 2: Design an Agent That Uses Your Skill

Now you'll create an agent that can consult your migration skill and guide developers through the modernization process.

> Reference: [Create custom agents (GitHub Docs)](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents)

1. Create `.github/agents/modernization-expert.agent.md`
2. Design your agent in any format you prefer. Include guidance on:
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

<!-- TODO: Validate if true -->
> [!TIP]
> Your agent can reference the security agent too. Consider adding a principle: "Consult the security agent for vulnerability fixes during migration."

### Step 3: Test Your Agent and Skill Together

Let's verify your agent can guide the PartsCatalogAPI migration:

1. Open Copilot Chat (`Ctrl+I`)
2. Use the agent dropdown to select your modernization agent
<!-- TODO: Improve -->
3. Ask the agent to analyze the ProductsController and provide a migration plan. Include details about:
    - The current code structure (inherits from ApiController, returns IHttpActionResult, uses sync database calls)
    - What needs to change and why
    - A modernized version using .NET 10 patterns
    - Dependencies that need updating
    - How to validate the changes work
<!-- TODO: Validate if true -->
4. **Evaluate the response:**
    - Does the agent reference the migration skill?
    - Does it provide specific code examples?
    - Does it explain breaking changes clearly?
    - Does it mention async/await requirements?
    - Does it note EF Core migration needs?

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
    - Create `migration-plan.md`
    - Paste the agent's response
    - Review and refine with your team's specific needs

## Understanding Skills vs Agents

### When to Create a Skill

Use skills when you have:
- **Reusable knowledge** that applies across multiple projects
- **Reference information** like API mappings, breaking changes
- **Best practices** that don't change per project
- **Domain expertise** multiple agents might need

Examples:
- `.NET Framework migration patterns` (this exercise)
- `TypeScript coding standards`
- `Azure architecture patterns`
- `API security checklist`

### When to Create an Agent

Use agents when you need:
- **Specialized workflows** with specific processes
- **Role-based assistance** (security auditor, migration guide)
- **Contextual decision-making** that varies per situation
- **Personality/communication style** for different tasks

Examples:
- `Security Modernization Expert` (Exercise 1)
- `.NET Modernization Expert` (this exercise)
- `Code Review Agent`
- `Documentation Writer`

### Can Agents Work Together?

**Yes!** Agents can reference each other:

```
Select the modernization agent: Plan the ProductsController migration
Switch to the security agent: Review the plan for security issues
Switch back to the modernization agent: Update the plan based on security feedback
```

This creates a **multi-agent workflow** where specialized agents collaborate!

## Success Criteria

- [ ] Created modernization skill in `.github/skills/dotnet-framework-migration/SKILL.md`
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

## Next Steps

You now have:
- ✅ A security agent that identifies vulnerabilities
- ✅ A migration skill with .NET Framework → .NET 10 knowledge
- ✅ A modernization agent that guides the migration process
- ✅ A complete migration plan for PartsCatalogAPI

---

**Ready to execute the migration?**  
Proceed to [Exercise 3: Execute the Migration](./03-execute-migration.md).
