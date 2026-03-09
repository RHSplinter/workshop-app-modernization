# App Modernization with GitHub Copilot Custom Agents & Skills

Welcome to this hands-on workshop! Transform complex modernization projects into guided, confident migrations using custom GitHub Copilot agents and skills.

## Workshop Overview

Learn to build custom GitHub Copilot agents and skills that transform risky modernization projects from weeks into hours. You'll create security-aware agents that audit your codebase, identify vulnerabilities, and execute framework upgrades while maintaining context across hundreds of files.

Official references:
- [Create custom agents (GitHub Docs)](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents)
- [Create skills (GitHub Docs)](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-skills)

**Duration:** 2 hours  
**Format:** Hands-on coding with your own fork

### What You'll Build

In this workshop, you'll modernize a legacy .NET Framework 4.8 Web API application to .NET 10 by:

1. Creating a **Security Agent** that identifies vulnerabilities and compliance issues
2. Building a **Modernization Skill** that encodes migration best practices
3. Using these tools to **execute a complete framework upgrade** with confidence

### Learning Objectives

By the end of this workshop, you will be able to:

1. **Create custom Copilot agents** - Build AI assistants with domain expertise that understand your security requirements and modernization strategy
2. **Build reusable skills** - Encode best practices into skills that make knowledge reusable across projects
3. **Execute safe migrations** - Use agents to maintain context through complex refactors and cascading changes
4. **Understand when and why** - Know when custom agents beat generic AI for complex tasks

## Why Custom Agents for Modernization?

Generic Copilot is great for code completion, but complex modernization requires:

- **Context Retention** - Agents remember your full migration strategy across hundreds of files
- **Domain Expertise** - Skills encode security standards and patterns that generic LLMs might miss
- **Consistency** - Same patterns applied across entire codebase
- **Teachability** - Skills improve as you refine them, like training a team member
- **Auditability** - Clear reasoning for each suggested change

## Prerequisites

Before attending this workshop, please ensure you have:

- [ ] A GitHub account with an active **Copilot Pro, Pro+, Business, or Enterprise** subscription
- [ ] **.NET 8 SDK or later** installed (we'll target .NET 10)
- [ ] **Visual Studio Code** with the GitHub Copilot extension enabled
- [ ] **Git** installed and configured
- [ ] Basic familiarity with C# and ASP.NET

> [!NOTE]
> If you are using Copilot Business or Copilot Enterprise, ensure your admin has enabled custom agents and skills features.

## Workshop Exercises

| Exercise | Duration | Topic | Description |
|----------|----------|-------|-------------|
| [0. Prerequisites][ex0] | 10 min | Setup | Fork the repo and verify environment |
| [1. Security Agent][ex1] | 25 min | Security Analysis | Create a security agent and audit the legacy app |
| [2. Modernization Skill & Agent][ex2] | 30 min | Migration Strategy | Build reusable modernization expertise |
| [3. Execute Migration][ex3] | 35 min | Hands-on Migration | Modernize the application to .NET 10 |
| [4. Review][ex4] | 15 min | Summary | Review results and discuss real-world applications |

[ex0]: ./00-prereqs.md
[ex1]: ./01-security-agent.md
[ex2]: ./02-modernization-skill.md
[ex3]: ./03-execute-migration.md
[ex4]: ./04-review.md

## The Sample Application

You'll work with a **Product Catalog API** - a realistic .NET Framework 4.8 Web API with intentional security issues and legacy patterns:

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

- **During the workshop**: Raise your hand or use the chat to ask questions
- **After the workshop**: Open an issue in this repository
- **GitHub Copilot Docs**: [docs.github.com/copilot](https://docs.github.com/en/copilot)

---

*Happy coding with GitHub Copilot! 🚀*

[ex0]: ./00-prereqs.md
[ex1]: ./01-first-exercise.md
[ex2]: ./02-second-exercise.md
[ex3]: ./03-review.md
