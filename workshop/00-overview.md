# App Modernization with GitHub Copilot Custom Agents & Skills

Welcome to this hands-on workshop! Transform complex modernization projects into guided, confident migrations using custom GitHub Copilot agents and skills.

## Workshop Overview

Learn to build custom GitHub Copilot agents and skills that transform risky modernization projects from weeks into hours. You'll create security-aware agents that audit your codebase, identify vulnerabilities, and execute framework upgrades while maintaining context across hundreds of files.

**Duration:** ~1.5 hours  
**Format:** Hands-on coding with your own repository

### What You'll Build

In this workshop, you'll modernize a legacy .NET Framework 4.8 Web API application to .NET 10 by:

1. Creating a **Security Agent** that identifies vulnerabilities and compliance issues
2. Building a **Migration Skill** that encodes migration best practices
3. Using these tools to **execute a complete framework upgrade** with confidence

## Workshop Exercises

| Step | Exercise | Duration | Topic | Description |
|------|----------|----------|-------|-------------|
| 01 | [Prerequisites](/workshop/01-prereqs.md) | 10 min | Setup | Copy the repo and verify environment |
| 02 | [Security Agent](/workshop/02-security-agent.md) | 20 min | Security Analysis | Create a security agent and audit the legacy app |
| 03 | [Modernization Skill & Agent](/workshop/03-migration-skill.md) | 20 min | Migration Strategy | Build reusable modernization expertise |
| 04 | [Execute Migration](/workshop/04-execute-migration.md) | 25 min | Hands-on Migration | Modernize the application to .NET 10 |
| 05 | [Review](/workshop/05-review.md) | 5 min | Summary | Review results and discuss real-world applications |

## Learning Objectives

By the end of this workshop, you will be able to:

1. **Create custom Copilot agents** - Build AI assistants with domain expertise that understand your security requirements and modernization strategy
2. **Build reusable skills** - Encode best practices into skills that make knowledge reusable across projects
3. **Execute safe migrations** - Use agents to maintain context through complex refactors and cascading changes
4. **Understand when and why** - Know when custom agents beat generic AI for complex tasks

## Why Custom Agents for Modernization?

Generic Copilot agents are great for code completion, but complex modernization requires:

- **Context Retention** - Agents remember your full migration strategy across hundreds of files
- **Domain Expertise** - Skills encode security standards and patterns that generic LLMs might miss
- **Consistency** - Same patterns applied across entire codebase
- **Teachability** - Skills improve as you refine them, like training a team member
- **Auditability** - Clear reasoning for each suggested change

## The Sample Application

You'll work with a **Parts Catalog API** - a realistic .NET Framework 4.8 Web API with intentional security issues and legacy patterns:

- **Framework:** .NET Framework 4.8 Web API
- **Data Access:** Entity Framework 6 with SQL Server
- **Size:** ~500 lines of code
- **Issues:** SQL injection, outdated packages, missing auth, synchronous patterns

## Tips for Success

1. **Let agents explore first** - Ask them to analyze the codebase before making changes
2. **Review before accepting** - Agents are powerful but you're the expert
3. **Iterate on skills** - Refine your skill definitions as you learn what works
4. **Ask "why"** - Agents can explain their reasoning for each change
5. **Don't rush** - Understanding the process is more valuable than finishing fast

## Support

- **During the workshop**: Reach out to the proctors at any time for questions or guidance
- **After the workshop**: Open an issue in this repository
- **GitHub Copilot Docs**: [docs.github.com/copilot](https://docs.github.com/en/copilot)
- **Awesome Copilot**: [github.com/github/awesome-copilot](https://github.com/github/awesome-copilot)

---

*Happy coding with GitHub Copilot! 🚀*
