# Prerequisites and Setup

Before starting this workshop, make sure you have the following set up. This should take about 10 minutes.

Helpful references:
- [Create custom agents (GitHub Docs)](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents)
- [Create skills (GitHub Docs)](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-skills)

## Required Tools

- [ ] **GitHub Account** with an active Copilot subscription (Pro, Pro+, Business, or Enterprise)
- [ ] **Git** installed and configured
- [ ] **Visual Studio Code** with GitHub Copilot extension enabled
- [ ] **.NET 8 SDK or later** installed on your machine
- [ ] **SQL Server Express** or **SQL Server LocalDB** (comes with Visual Studio)

> [!NOTE]
> We're migrating TO .NET 10 but you need a modern SDK installed to build the target. The source application is .NET Framework 4.8.

## Environment Setup

### 1. Fork and Clone the Repository

1. Fork this repository to your GitHub account (click **Fork** button at top right)
2. Clone your fork locally:

```bash
git clone https://github.com/RHSplinter/workshop-app-modernization.git
cd workshop-app-modernization
```

### 2. Open in Visual Studio Code

```bash
code .
```

### 3. Verify GitHub Copilot is Active

1. Look for the Copilot icon in the bottom-right status bar of VS Code
2. Open any file and start typing a comment - you should see inline suggestions
3. Test the Copilot chat by pressing `Ctrl+I` (or `Cmd+I` on Mac)

> [!TIP]
> If Copilot isn't working, check:
> - Your subscription is active at [github.com/settings/copilot](https://github.com/settings/copilot)
> - The Copilot extension is installed and enabled in VS Code
> - You're signed into GitHub in VS Code (bottom-left account icon)

### 4. Verify .NET Installation

Run the following to confirm you have .NET SDK 8 or later:

```bash
dotnet --version
```

You should see version 8.0.0 or higher.

Check installed SDKs:

```bash
dotnet --list-sdks
```

> [!TIP]
> If you don't have .NET 8+, download it from [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

### 5. Verify SQL Server

Check if LocalDB is available:

```bash
sqllocaldb info
```

If this returns "MSSQLLocalDB" or similar, you're ready!

If not, you can:
- Download SQL Server Express from [microsoft.com/sql-server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Or use SQL Server in a Docker container

### 6. Explore the Legacy Application

The sample application is in the `src/ProductCatalogAPI` folder

Take a moment to explore the structure:

```
ProductCatalogAPI/
├── App_Start/           # Web API configuration
├── Controllers/         # API controllers
├── Data/                # EF6 DbContext
├── Models/              # Data models
├── packages.config      # Old-style package management
└── Web.config           # Legacy configuration
```

## Understanding the Challenge

You're about to modernize a .NET Framework 4.8 Web API that has:

- ❌ Known security vulnerabilities (CVEs in old packages)
- ❌ No authentication or authorization
- ❌ SQL injection risks
- ❌ Synchronous database calls
- ❌ Legacy configuration (`web.config`)
- ❌ Old package management (`packages.config`)

Your goal: Transform this into a secure, modern .NET 10 API with:

- ✅ Latest packages with security patches
- ✅ Modern authentication middleware
- ✅ Parameterized queries
- ✅ Async/await patterns
- ✅ Modern configuration (`appsettings.json`)
- ✅ SDK-style project files

## Quick Test: Compare Generic vs Custom Agents

Before we dive in, let's see why custom agents matter:

1. Open Copilot Chat (`Ctrl+I`)
2. Ask: "How do I upgrade this to .NET 10?"
3. Notice the generic advice

By the end of this workshop, you'll have an agent that:
- Understands YOUR specific security requirements
- Knows YOUR migration patterns
- Maintains context across ALL your files
- Explains WHY each change is needed

## Success Criteria

- [ ] Repository forked and cloned
- [ ] VS Code open with the project
- [ ] GitHub Copilot active and responding
- [ ] .NET SDK 10 installed and working
- [ ] SQL Server LocalDB available
- [ ] You understand the challenge ahead

---

**Ready to begin?**  
Proceed to [Exercise 1: Create a Security Agent](./01-security-agent.md).
