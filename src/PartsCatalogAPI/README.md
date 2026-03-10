# Parts Catalog API - Legacy .NET Framework 4.8

A legacy ASP.NET Web API application built on .NET Framework 4.8 for managing an auto parts catalog. This application is intentionally designed with security vulnerabilities and legacy patterns for use in the **App Modernization with GitHub Copilot** workshop.

## 🎯 Purpose

This project serves as a hands-on learning tool for:
- Migrating from .NET Framework 4.8 to .NET 10
- Identifying and fixing security vulnerabilities using custom GitHub Copilot agents
- Modernizing legacy code patterns with GitHub Copilot skills
- Understanding the value of custom agents over generic AI assistance

## 📋 Prerequisites

- Visual Studio 2019 or later / Visual Studio Code with .NET extension
- .NET Framework 4.8 Developer Pack
- SQL Server LocalDB or SQL Server Express
- GitHub Copilot subscription with access to custom agents

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/YOUR-USERNAME/workshop-app-modernization.git
cd workshop-app-modernization/src/PartsCatalogAPI
```

### 2. Restore Packages

Using Visual Studio:
- Open `PartsCatalogAPI.csproj`
- Right-click on the solution and select "Restore NuGet Packages"

Using Command Line:
```bash
nuget restore PartsCatalogAPI.csproj
```

### 3. Build the Project

```bash
msbuild PartsCatalogAPI.csproj /p:Configuration=Release
```

### 4. Run the Application

Using Visual Studio:
- Press F5 to run with debugging
- The API will start at `http://localhost:5000/`

Using IIS Express:
```bash
"C:\Program Files (x86)\IIS Express\iisexpress.exe" /path:C:\path\to\PartsCatalogAPI /port:5000
```

### 5. Test the API

Once running, you can access:
- All Products: `http://localhost:5000/api/Products`
- All Categories: `http://localhost:5000/api/Categories`
- Search Products: `http://localhost:5000/api/Products/Search?name=brake`

## 🗂️ Project Structure

```
PartsCatalogAPI/
├── App_Start/
│   └── WebApiConfig.cs          # Web API routing configuration
├── Controllers/
│   ├── ProductsController.cs    # Product management endpoints
│   └── CategoriesController.cs  # Category management endpoints
├── Data/
│   ├── PartsCatalogContext.cs   # Entity Framework 6 DbContext
│   └── PartsCatalogInitializer.cs # Database seeding
├── Models/
│   ├── Product.cs               # Product entity
│   └── Category.cs              # Category entity
├── Properties/
│   └── AssemblyInfo.cs
├── Global.asax                  # Application startup
├── Web.config                   # Application configuration
└── packages.config              # NuGet package references
```

## 🔴 Known Security Issues (Intentional)

This application contains **intentional security vulnerabilities** for educational purposes:

### 1. SQL Injection Vulnerabilities
- **Location**: `ProductsController.SearchProducts()`, `CategoriesController.GetCategoryByName()`
- **Issue**: User input is directly concatenated into SQL queries
- **Risk**: Attackers can execute arbitrary SQL commands

### 2. Missing Authentication/Authorization
- **Location**: All controller endpoints
- **Issue**: No `[Authorize]` attributes, anyone can access all endpoints
- **Risk**: Unauthorized users can view, create, update, and delete data

### 3. Hardcoded Credentials
- **Location**: `Web.config` appSettings
- **Issue**: Admin credentials and API keys stored in plain text
- **Risk**: Credentials exposed in source control and config files

### 4. Outdated Dependencies
- **Location**: `packages.config`
- **Issue**: Using Newtonsoft.Json 9.0.1 (from 2016) with known vulnerabilities
- **Risk**: Known security vulnerabilities in dependencies

### 5. Sensitive Data Exposure
- **Location**: `ProductsController.GetDebugInfo()`
- **Issue**: Debug endpoint exposes database names, credentials, and API keys
- **Risk**: Information leakage aids attackers

### 6. Insecure Configuration
- **Location**: `Web.config`
- **Issue**: Custom errors disabled, connection string with credentials, no HTTPS enforcement
- **Risk**: Stack traces exposed, credentials in config, MITM attacks possible

### 7. Synchronous Database Operations
- **Location**: All controller methods using `SaveChanges()`
- **Issue**: Blocking I/O operations instead of async/await
- **Risk**: Poor performance and scalability

## 📚 Workshop Flow

During the workshop, you will:

1. **Exercise 1 (20 min)**: Create a custom Security Analysis Agent
   - Define expertise in security vulnerability detection
   - Run analysis on this codebase
   - Generate a detailed security report

2. **Exercise 2 (25 min)**: Build a .NET Framework Migration Skill
   - Create reusable knowledge for .NET migrations
   - Package patterns, breaking changes, and best practices
   - Build an agent that leverages the skill

3. **Exercise 3 (25 min)**: Execute the Migration
   - Convert project to .NET 10
   - Modernize code patterns (async/await, dependency injection)
   - Fix security vulnerabilities
   - Migrate Entity Framework 6 to EF Core 9
   - Update configuration from Web.config to appsettings.json

4. **Exercise 4 (5 min)**: Review and Reflect
   - Compare results with and without custom agents
   - Discuss time savings and accuracy improvements
   - Explore real-world applications

## 🎓 Learning Objectives

By the end of this workshop, you will understand:

- ✅ How to create custom GitHub Copilot agents with specific expertise
- ✅ The difference between agents and skills
- ✅ Techniques for migrating .NET Framework to .NET 10
- ✅ Modern security patterns in ASP.NET Core
- ✅ The advantages of custom agents over generic AI assistance

## 🔧 Technologies Used

### Current (Legacy)
- .NET Framework 4.8
- ASP.NET Web API 2
- Entity Framework 6
- System.Web namespace
- Web.config configuration
- Synchronous database operations

### Target (After Migration)
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 9
- Microsoft.AspNetCore namespace
- appsettings.json configuration
- Async/await patterns
- Dependency injection
- JWT authentication

## ⚠️ Important Notes

1. **DO NOT USE IN PRODUCTION**: This application contains intentional security vulnerabilities
2. **Educational Purpose Only**: Designed specifically for the App Modernization workshop
3. **No Real Data**: Use only test data, never production or sensitive information
4. **Local Development Only**: Run only in isolated local development environments

## 📖 Additional Resources

- [Workshop Guide](../../workshop/00-overview.md)
- [.NET Upgrade Assistant](https://dotnet.microsoft.com/platform/upgrade-assistant)
- [ASP.NET Core Migration Documentation](https://learn.microsoft.com/aspnet/core/migration/)
- [GitHub Copilot Documentation](https://docs.github.com/copilot)

## 📝 License

This project is provided for educational purposes as part of the GitHub Copilot Dev Days workshop.

## 🤝 Contributing

This is a workshop sample project. If you find issues or have suggestions for improvement, please open an issue in the repository.

---

**Ready to modernize?** Head to the [workshop guide](../../workshop/00-overview.md) to get started!
