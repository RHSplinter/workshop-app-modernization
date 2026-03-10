# Parts Catalog API - Legacy .NET Framework 4.8

A legacy ASP.NET Web API application built on .NET Framework 4.8 for managing an auto parts catalog. This application is intentionally designed with security vulnerabilities and legacy patterns for use in the **App Modernization with GitHub Copilot** workshop.

This project is based on the [Parts Unlimited](https://github.com/microsoft/PartsUnlimitedE2E/tree/master/PartsUnlimited-aspnet45) project by Microsoft; specifically updated for the purpose of this workshop.

## 🎯 Purpose

This project serves as a hands-on learning tool for:
- Migrating from .NET Framework 4.8 to .NET 10
- Identifying and fixing security vulnerabilities using custom GitHub Copilot agents
- Modernizing legacy code patterns with GitHub Copilot skills
- Understanding the value of custom agents over generic AI assistance

## 📋 Prerequisites

- Visual Studio 2019 or later / Visual Studio Code with .NET extension
- .NET 10 SDK (required to build after migration)
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
dotnet restore
```

### 3. Build the Project

> [!NOTE]
> Requires the application to be migrated to .NET 10.

```bash
dotnet build
```

### 4. Run the Application

1. Run `dotnet run`
2. The API will start at `http://localhost:5000/`

### 5. Test the API

Once running, you can access:
- All Products: `http://localhost:5000/api/Products`
- All Categories: `http://localhost:5000/api/Categories`
- Search Products: `http://localhost:5000/api/Products/Search?name=brake`

## 🗂️ Project Structure

```
PartsCatalogAPI/
├── App_Start/
│   └── WebApiConfig.cs             # Web API routing configuration
├── Controllers/
│   ├── ProductsController.cs       # Product management endpoints
│   └── CategoriesController.cs     # Category management endpoints
├── Data/
│   ├── PartsCatalogContext.cs      # Entity Framework 6 DbContext
│   └── PartsCatalogInitializer.cs  # Database seeding
├── Models/
│   ├── Product.cs                  # Product entity
│   └── Category.cs                 # Category entity
├── Properties/
│   └── AssemblyInfo.cs
├── Global.asax                     # Application startup
├── Web.config                      # Application configuration
└── packages.config                 # NuGet package references
```

## 🔧 Technologies Used

### Current (Legacy)
- .NET Framework 4.8
- ASP.NET Web API 2
- Entity Framework 6
- System.Web namespace
- Web.config configuration
- Synchronous database operations

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
