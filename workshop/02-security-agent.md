# Exercise 1: Security Analysis with Custom Agents

In this exercise, you'll discover the power of custom agents by comparing generic Copilot security analysis with a specialized security agent. You'll see firsthand why custom agents matter for complex tasks like security auditing.

## Why a Security Agent?

Generic Copilot agents don't know:
- **Your organization's security standards** (OWASP compliance, specific CVE thresholds)
- **Your framework-specific risks** (Framework vulnerabilities, legacy authentication patterns)
- **Your security priorities** (what must be fixed before migration vs what can wait)
- **Your reporting requirements** (how to categorize and communicate findings)

A custom security agent encodes this knowledge and applies it consistently across your codebase.

## Objectives

- Run a baseline security analysis with a generic agent
- Create a custom security agent with specialized knowledge
- Compare results to see the "before/after" difference
- Generate a prioritized vulnerability report for migration planning

## Part 1: Baseline Security Scan

Let's start by seeing what a generic agent finds in our PartsCatalogAPI application.

### Step 1: Review the Application Structure

The `src/PartsCatalogAPI` folder contains a .NET Framework 4.8 Web API with:
- **Controllers**: [ProductsController.cs](../src/PartsCatalogAPI/Controllers/ProductsController.cs), [CategoriesController.cs](../src/PartsCatalogAPI/Controllers/CategoriesController.cs)
- **Data Layer**: Entity Framework 6 DbContext in [PartsCatalogContext.cs](../src/PartsCatalogAPI/Data/PartsCatalogContext.cs)
- **Models**: Product and Category classes
- **Configuration**: [Web.config](../src/PartsCatalogAPI/Web.config) with connection strings and app settings
- **Dependencies**: [packages.config](../src/PartsCatalogAPI/packages.config) with legacy package references

### Step 2: Run a Generic Security Analysis

1. Open GitHub Copilot Chat (`Ctrl+Shift+I` on Windows or `Cmd+Shift+I` on Mac)
2. In agent mode, request a comprehensive security audit of the controllers.
3. Observe the results:
    - How comprehensive is the analysis?
    - Does it check [Web.config](../src/PartsCatalogAPI/Web.config) for hardcoded credentials?
    - Does it identify the outdated Newtonsoft.Json package (v9.0.1 - released 2016)?
    - Does it recognize patterns specific to .NET Framework security risks?

> [!TIP]
> Save this response - you'll compare it with the custom agent results later.

## Part 2: Create a Custom Security Agent

### Step 1: Design Your Security Agent

Now you'll create a specialized agent that knows how to scan .NET Framework applications for security vulnerabilities with migration context. GitHub Copilot supports [creating custom agents](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents) that can be tailored to your specific needs, with detailed [configuration options](https://docs.github.com/en/copilot/reference/custom-agents-configuration) available in the official documentation.

> [!TIP]
> The Awesome Copilot repository (github.com/github/awesome-copilot) is a community-driven toolkit for Copilot, which includes custom agents, skills and more!

1. Create a new folder structure:
    ```
    .github/
    └── agents/
        └── security-expert.agent.md
    ```
2. Design your agent definition. Include guidance on:
    - Security vulnerability types to detect (SQL injection, auth issues, hardcoded secrets, package CVEs, input validation)
    - Compliance requirements (OWASP, CVE)
    - Reporting expectations (severity levels, remediation, migration timing)
    - Quality rules (e.g.: include CVE references when possible, explain why each issue matters, suggest modern .NET alternatives)
3. Save your agent definition

### Step 2: Verify and Test Your Agent

1. Open GitHub Copilot Chat
2. Use the agent selector dropdown in the chat interface and choose your custom agent
3. You should see your agent name appear (e.g., "Security Expert")

**Troubleshooting:**  
If you don't see it, try:
- Reload VS Code window (`Ctrl+Shift+P` → "Reload Window")
- Check that the file path is exactly `.github/agents/security-expert.agent.md`
- Verify the YAML frontmatter is valid

## Part 3: Specialized Security Scan

### Step 1: Run Targeted Security Scan with Your Agent

Now let's see how your custom agent performs on the same analysis:
1. Open Copilot Chat (`Ctrl+Shift+I`)
2. Use the agent dropdown to select your custom agent
3. Request another comprehensive security audit of the controllers.
4. **Compare the results** with your baseline scan from Part 1:  
    **What improved?**
    - Does the agent provide more specific context and remediation?
    - Is the SQL injection vulnerability in `SearchProducts()` explained better?
    - Are findings categorized by severity?
    - Does it suggest modern .NET alternatives?

### Step 2: Deep Dive on Critical Issues

Ask your agent to analyze the `SearchProducts()` and `GetCategoryByName()` methods. Request:
- An explanation of why these methods are vulnerable to SQL injection
- The potential impact of these vulnerabilities
- How to fix them during migration to .NET 10 (including code examples)
- Before/after code comparisons

### Step 3: Generate Migration-Ready Security Report

1. Ask your agent to create a structured security report that includes:
    - An executive summary with vulnerability counts organized by severity
    - Critical findings with specific file paths referenced
    - Code snippets showing the vulnerable patterns
    - A remediation roadmap indicating what to fix before, during, and after migration to .NET 10
    - Package upgrade recommendations with specific version numbers
2. Request the output to be saved to `security-audit.md`.

## Reflection: What's Different?

### Without Custom Agent (Baseline)

- ❌ Generic advice about "updating packages"
- ❌ Missing context about .NET Framework 4.8 specific issues
- ❌ No migration-aware prioritization
- ❌ Inconsistent reporting format
- ❌ May miss framework-specific vulnerabilities

### With Custom Agent

- ✅ Targeted .NET Framework security analysis
- ✅ Specific CVE identification with package versions
- ✅ Migration-context prioritization (fix before/during/after)
- ✅ Consistent, structured reporting
- ✅ Actionable remediation with code examples
- ✅ Understanding of legacy patterns vs modern alternatives

## Success Criteria

- [ ] Completed baseline security scan with generic agent
- [ ] Created custom security agent with specialized expertise
- [ ] Agent is recognized in Copilot Chat (appears in the agent dropdown)
- [ ] Ran comparative analysis showing improved results
- [ ] Generated structured security report

## Troubleshooting

**Agent not appearing in the chat dropdown?**
- Check file path: Must be `.github/agents/*.agent.md`
- Review the GitHub docs for expected custom agent formatting
- Reload VS Code window
- Ensure your intended agent name appears in the agent definition

**Agent gives generic responses similar to baseline?**
- Review the agent definition - be more specific about expertise and process
- Provide more detailed and relevant instructions
- Ask more targeted questions with specific file paths

**Not seeing all security issues?**
- Use references to directories and files in prompts.
- Ask the agent to specifically look for patterns (SQL injection, missing auth, hardcoded secrets)
- Review the agent's expertise section - does it cover these areas?

**Responses are too brief?**
- Update your agent's reporting standards to require detailed explanations
- Ask follow-up questions referencing specific files and line numbers

## Reflection Questions

1. What security issues did your custom agent find that the generic agent missed?
2. How does having a specialized agent change the quality and depth of analysis?
3. What other security expertise could you encode for your organization's needs?
4. How would you evolve this agent based on your company's security standards?
5. What role will this security context play during the actual migration process?
