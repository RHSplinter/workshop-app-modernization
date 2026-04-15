# App Modernization with GitHub Copilot

A hands-on workshop for learning how to modernize .NET Framework applications to .NET 10 using custom GitHub Copilot agents and skills.

## 🎯 Workshop Overview

In this workshop, you'll learn to leverage custom GitHub Copilot agents and skills to modernize a legacy .NET Framework 4.8 Web API application. You'll discover how specialized agents significantly improve code migration, security analysis, and modernization tasks compared to generic AI assistance.

## 🚀 Start the Workshop

**To begin the workshop:**
1. Copy the exercise to your account
2. Checkout the [published workshop site](https://rhsplinter.github.io/workshop-app-modernization)

[![](https://img.shields.io/badge/Copy%20Exercise-%E2%86%92-1f883d?style=for-the-badge&logo=github&labelColor=197935)](https://github.com/new?template_owner=RHSplinter&template_name=workshop-app-modernization&owner=%40me&name=workshop-app-modernization&description=Exercise:+Modernize+Applications+With+Custom+Agents+And+Skills&visibility=public)


## 📋 What You'll Learn

- ✅ Create custom GitHub Copilot agents with domain-specific expertise
- ✅ Build reusable skills that can be shared across agents
- ✅ Identify security vulnerabilities in legacy code
- ✅ Migrate .NET Framework 4.8 applications to .NET 10
- ✅ Modernize Entity Framework 6 to EF Core 9
- ✅ Compare the effectiveness of custom agents vs. generic Copilot agent mode

## 🧪 Sample Application

This workshop includes a real-world sample: a legacy Parts Catalog API with intentional security vulnerabilities and outdated patterns. You'll apply the skills you learn to analyze and modernize this application step-by-step throughout the workshop.

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

## License

This project is licensed under the terms of the MIT open source license. Please refer to the [LICENSE](./LICENSE) for the full terms.

## Support

If you have questions, please open an issue.
