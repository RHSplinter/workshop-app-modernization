# Exercise 1: Create a Security Agent

**Duration:** 25 minutes

In this exercise, you'll create a custom GitHub Copilot agent that specializes in security analysis for .NET applications. This agent will identify vulnerabilities, outdated packages, and insecure coding patterns.

## Why a Security Agent?

Generic Copilot doesn't know:
- **Your organization's security standards** (OWASP compliance, specific CVE thresholds)
- **Your framework-specific risks** (.NET Framework security issues vs .NET Core)
- **Your security priorities** (what's critical vs nice-to-have)

A custom security agent encodes this knowledge and applies it consistently.

## Objectives

- Create a custom agent definition file
- Define security expertise and scanning capabilities
- Run a comprehensive security audit
- Generate a prioritized vulnerability report

## Part 1: Create the Security Agent (10 minutes)

### Step 1: Create the Agent File

1. In VS Code, create a new folder structure:
   ```
   .github/
   └── agents/
       └── security-modernization.agent.md
   ```

2. Open the new file and add the following agent definition:

```markdown
---
name: Security Modernization Expert
description: Specialized agent for identifying security vulnerabilities in .NET Framework applications during modernization
expertise:
  - .NET Framework security patterns and anti-patterns
  - Common Vulnerabilities and Exposures (CVE) analysis
  - OWASP Top 10 compliance
  - Package dependency security auditing
  - Authentication and authorization implementation
---

# Security Modernization Expert

You are a security expert specializing in .NET application modernization with a focus on identifying and remediating security vulnerabilities.

## Your Expertise

### Security Analysis
- Identify SQL injection vulnerabilities (string concatenation in queries)
- Detect missing authentication and authorization
- Find hardcoded credentials and connection strings
- Audit packages for known CVEs
- Check for missing input validation
- Identify insecure cryptographic practices

### .NET Framework Specific Issues
- Legacy `System.Web` security issues
- Missing HTTPS enforcement
- Weak authentication schemes (Basic, Forms without proper configuration)
- Session management vulnerabilities
- XML external entity (XXE) vulnerabilities
- Deserialization vulnerabilities

### Reporting Standards
When analyzing code, provide:
1. **Severity** (Critical, High, Medium, Low)
2. **Issue Description** (what's wrong and why it matters)
3. **Location** (file, line number, method)
4. **Remediation** (specific fix for this codebase)
5. **Priority** (should this be fixed before migration, during, or after?)

## Your Process

1. **Scan** - Analyze all source files for security issues
2. **Prioritize** - Rank by severity and impact on migration
3. **Categorize** - Group by type (injection, auth, crypto, dependencies)
4. **Recommend** - Provide actionable fixes with code examples
5. **Report** - Generate a structured markdown report

## Important Rules

- Always provide specific file paths and line numbers
- Include CVE numbers when referencing known vulnerabilities
- Consider the migration context (some issues can be fixed during modernization)
- Don't just list issues - explain WHY each matters
- Provide modern .NET alternatives for legacy patterns
```

3. Save the file (`Ctrl+S`)

> [!TIP]
> The frontmatter (YAML between `---`) defines metadata. The markdown body is the agent's personality and instructions.

### Step 2: Verify the Agent is Recognized

1. Open GitHub Copilot Chat
2. Type `@` and look for your custom agent in the suggestions
3. You should see "Security Modernization Expert" appear

> [!NOTE]
> If you don't see it, try:
> - Reload VS Code window (`Ctrl+Shift+P` → "Reload Window")
> - Check that the file path is exactly `.github/agents/security-modernization.agent.md`
> - Verify the YAML frontmatter is valid

## Part 2: Run Security Audit (15 minutes)

### Step 3: Scan the Legacy Application

Now let's use your custom agent to audit the Product Catalog API.

1. Open Copilot Chat (`Ctrl+I`)
2. Invoke your custom agent:

```
@security-modernization Please perform a comprehensive security audit of the ProductCatalogAPI application. 

Analyze:
- All controller files for injection vulnerabilities
- Package dependencies for known CVEs
- Authentication and authorization implementation
- Data access patterns for security issues
- Configuration files for exposed secrets

Provide a prioritized report with:
1. Critical issues that must be fixed
2. High priority issues for the migration
3. Medium/Low issues that can be addressed later

For each issue, include specific file paths and remediation guidance.
```

3. Review the agent's response

> [!TIP]
> The agent should identify:
> - SQL injection in ProductsController (string concatenation)
> - Outdated packages with CVEs (Newtonsoft.Json, Entity Framework)
> - Missing authentication/authorization attributes
> - Hardcoded connection string in Web.config
> - No HTTPS enforcement
> - Synchronous database calls (while not security, impacts reliability)

### Step 4: Generate Security Report

Ask your agent to create a structured report:

```
@security-modernization Create a markdown security report that I can save to docs/security-audit.md. 

Include:
- Executive summary with vulnerability counts by severity
- Detailed findings with code snippets
- Remediation roadmap (what to fix when during migration)
- Before/after examples for critical issues
```

### Step 5: Save and Commit the Report

1. Create `docs/security-audit.md` with the generated content
2. Review the findings - do they make sense?
3. Commit the report:

```bash
git add .github/agents/security-modernization.agent.md
git add docs/security-audit.md
git commit -m "Add security agent and initial audit report"
```

## Understanding What Just Happened

### Without Custom Agent
- Generic advice about "updating packages"
- No context about your specific framework version
- Missing framework-specific vulnerabilities
- No prioritization for migration context

### With Custom Agent
- Targeted analysis of .NET Framework security issues
- Specific CVE identification
- Migration-aware prioritization
- Actionable remediation steps
- Consistent reporting format

## Common Findings You Should See

1. **Critical: SQL Injection** in `ProductsController.Search()` method
2. **Critical: Missing Authentication** - no `[Authorize]` attributes
3. **High: Outdated Packages** - Newtonsoft.Json 9.0.1 has known CVEs
4. **High: Hardcoded Connection String** in `Web.config`
5. **Medium: No HTTPS Enforcement** - API accepts HTTP requests
6. **Medium: No Input Validation** - controllers accept raw input

## Success Criteria

- [ ] Custom security agent created and recognized by Copilot
- [ ] Security audit completed with specific findings
- [ ] Security report generated with severity levels
- [ ] Report saved to `docs/security-audit.md`
- [ ] Changes committed to Git
- [ ] You understand why each finding matters

## Troubleshooting

**Agent not appearing in @ mentions?**
- Check file path: `.github/agents/*.agent.md`
- Validate YAML frontmatter syntax
- Reload VS Code window

**Agent gives generic responses?**
- Review the agent definition - it might need more specific instructions
- Ask more targeted questions with specific files/patterns
- Provide more context in your prompts

**Not seeing security issues?**
- Make sure you're analyzing the legacy .NET Framework app, not a modern one
- Ask the agent to specifically look for SQL injection and auth issues

## Reflection Questions

1. What security issues did you find that you might have missed without the agent?
2. How does the agent's prioritization help with migration planning?
3. What other security expertise could you encode in an agent for your organization?

---

**Ready to encode modernization expertise?** Proceed to [Exercise 2: Build a Modernization Skill](./02-modernization-skill.md).
