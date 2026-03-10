# App Modernization with GitHub Copilot

A hands-on workshop for learning how to modernize .NET Framework applications to .NET 10 using custom GitHub Copilot agents and skills.

## 🎯 Workshop Overview

In this workshop, you'll learn to leverage custom GitHub Copilot agents and skills to modernize a legacy .NET Framework 4.8 Web API application. You'll discover how specialized agents dramatically improve code migration, security analysis, and modernization tasks compared to generic AI assistance.

## 🚀 Start the Workshop

**To begin the workshop, start at [workshop/00-overview.md](./workshop/00-overview.md)**

Or visit the [published workshop site](https://rhsplinter.github.io/workshop-app-modernization).

## 📋 What You'll Learn

- ✅ Create custom GitHub Copilot agents with domain-specific expertise
- ✅ Build reusable skills that can be shared across agents
- ✅ Identify security vulnerabilities in legacy code
- ✅ Migrate .NET Framework 4.8 applications to .NET 10
- ✅ Modernize Entity Framework 6 to EF Core 9
- ✅ Implement async/await patterns and dependency injection
- ✅ Compare the effectiveness of custom agents vs. generic Copilot agent mode

## 🧪 Sample Application

This workshop includes a real-world sample: a legacy Parts Catalog API with intentional security vulnerabilities and outdated patterns. The application demonstrates common challenges in enterprise .NET Framework applications:

- SQL injection vulnerabilities
- Missing authentication/authorization
- Hardcoded credentials
- Outdated dependencies (e.g., Newtonsoft.Json 9.0.1)
- Synchronous database operations
- Legacy configuration patterns

👉 **See the sample project**: [src/PartsCatalogAPI](./src/PartsCatalogAPI)

## Repository Structure

```
├── docs/           # Published HTML site (GitHub Pages)
│   ├── index.html  # Landing page
│   ├── step.html   # Step viewer (renders workshop/ markdown)
│   ├── styles.css
│   ├── light-theme.css
│   └── theme-toggle.js
├── workshop/       # Workshop content (markdown)
│   ├── 00-overview.md
│   ├── 01-prereqs.md
│   ├── 02-security-agent.md
│   ├── 03-migration-skill.md
│   ├── 04-execute-migration.md
│   └── 05-review.md
├── .github/
│   ├── copilot-instructions.md
│   └── workflows/deploy.yml
├── README.md
└── LICENSE
```

## Publishing

The workshop site deploys automatically to GitHub Pages when you push to `main`. Enable GitHub Pages in your repository settings (Settings → Pages → Source: GitHub Actions).

## License

This project is licensed under the terms of the MIT open source license. Please refer to the [LICENSE](./LICENSE) for the full terms.

## Support

This project is provided as-is, and may be updated over time. If you have questions, please open an issue.
