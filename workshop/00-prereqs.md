# Prerequisites and Setup

Before starting this workshop, make sure you have the following set up. This should take about 10 minutes.

## Required Tools

- [ ] **GitHub Account** with an active Copilot subscription (Copilot Free is sufficient)
- [ ] **Git** installed and configured
- [ ] **Visual Studio Code** with GitHub Copilot extension enabled
- [ ] **.NET 10 SDK or later** installed on your machine
- [ ] **SQL Server Express** or **SQL Server LocalDB** (comes with Visual Studio)

## Environment Setup

### 1. Fork and Clone the Repository

1. Fork this repository to your GitHub account (click **Fork** button at top right)
2. Clone your fork locally:

```bash
git clone https://github.com/<YOUR-GITHUB-HANDLE>/workshop-app-modernization.git
cd workshop-app-modernization
```

### 2. Open in Visual Studio Code or Codespaces

Choose one option:

**Option A: Local VS Code**

```bash
code .
```

**Option B: GitHub Codespaces**

Open your fork on GitHub, click **Code** > **Codespaces** > **Create codespace on main**.

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

Run the following to confirm you have .NET SDK 10 or later:

```bash
dotnet --version
```

You should see version 10.0.0 or higher.

Check installed SDKs:

```bash
dotnet --list-sdks
```

> [!TIP]
> If you don't have .NET 10+, download it from [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

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

The sample application is in the `src/ProductCatalogAPI` folder.

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

## ✅ Checkpoint

- [ ] Repository forked and cloned
- [ ] VS Code open with the project
- [ ] GitHub Copilot active and responding
- [ ] .NET SDK 10 installed and working
- [ ] SQL Server LocalDB available
- [ ] You understand the challenge ahead
