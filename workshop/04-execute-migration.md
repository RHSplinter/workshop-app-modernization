# Exercise 3: Execute the Migration

Now it's time to put your custom agents to work! You'll execute the migration plan you created in Exercise 2, using your agents to guide each step.

## Objectives

- Use your migration plan to guide the actual code changes
- Execute each phase of the migration step-by-step with your agent
- Fix security vulnerabilities during modernization
- Validate that the modernized application works correctly

> [!NOTE]
> You have a migration plan (`migration-plan.md`) created in Exercise 2. In this exercise, you'll ask your modernization agent to execute it phase-by-phase.
> Your agents maintain context during the session. They "remember" what you've changed and ensure consistent patterns throughout.
> In VS Code, choose agents from the chat dropdown. Switch between agents as needed for each prompt.

## Executing Your Migration Plan

You created a comprehensive migration plan in Exercise 2. Now you'll execute it step-by-step with your agent.

### Key Principle: Execute Each Phase in Order

Your migration plan likely breaks down the work into phases (project structure, dependencies, code patterns, configuration, testing). For each phase:

1. **Open your migration plan**: Review `migration-plan.md` to see what the next phase is
2. **Share relevant portion with agent**: Open Copilot Chat and select your modernization agent
3. **Ask the agent to execute the phase**: Provide the context and phase description from your plan
4. **Apply the generated changes**: Make the code updates based on the agent's guidance
5. **Move to next phase**: Repeat for each phase in your plan

## Part 1: Starting the Migration

### Step 1: Review Your Migration Plan

Open `migration-plan.md` that you created in Exercise 2. Note:
- What are the phases outlined?
- What's the recommended order?
- What are the key dependencies?
- What files need to change in each phase?

### Step 2: Execute Phase 1 - Project Structure

1. Open Copilot Chat (`Ctrl+I`)
2. Select your **modernization agent** from the dropdown
3. Add the migration plan to the context (select the file in the chat window or reference the file name with '#{file-name}')
4. Ask the agent to execute the plan **step-by-step**. Using a step-by-step approach ensures that:
    - The agent only works on small changes at a time.
    - You can review the work between steps and adjust accordingly.
5. As the agent works through each phase, review the changes. Make changes when necessary or ask questions to the agent if unclear.

> [!WARNING]
> Don't commit actual passwords or API keys! Your security agent should remind you about this. Use:
> - User Secrets for local development
> - Azure Key Vault or environment variables for production

## Part 2: Testing & Validation

Once you've completed all phases of the migration plan, it's time to validate that everything works correctly.

### Step 1: Build and Test the Application

1. Run the build:
    ```bash
    dotnet restore
    dotnet build
    ```
2. **If you encounter build errors:**
    - Paste the error message to your modernization agent
    - Ask what the cause is and how to fix it
    - Your agents can help troubleshoot common issues like missing package references, namespace conflicts, or API compatibility problems
3. Run the application:
    ```bash
    dotnet run --project src/PartsCatalogAPI
    ```
    - The API should start on `https://localhost:5001` (or as configured)
    - Watch the console output for any startup errors

### Step 2: Validate All Endpoints

1. Navigate to: `https://localhost:5001/swagger`
2. Test each endpoint to ensure they work:
    - **GET** `/api/products` - Should return all products
    - **GET** `/api/products/{id}` - Should return single product
    - **GET** `/api/products/Search?name=brake` - **Critically**: Should work WITHOUT SQL injection!
    - **POST** `/api/products` - Should create a product
    - **PUT** `/api/products/{id}` - Should update a product
    - **DELETE** `/api/products/{id}` - Should delete a product
    - Repeat for Categories endpoints

> [!NOTE]
> If you added authentication in your migration plan, you'll need a valid JWT token to test protected endpoints.

### Step 3: Verify Security Fixes

1. **SQL Injection Test**: Try `GET /api/products/Search?name='; DROP TABLE Products; --`
    - ✅ **Expected**: Should return empty results or error, **NOT** execute SQL
    - ❌ **Old behavior**: Would have executed the malicious SQL
2. **Authentication Test** (if configured): Try calling POST/PUT/DELETE without a token
    - ✅ **Expected**: 401 Unauthorized
    - ❌ **Old behavior**: Would have allowed the operation

### Step 4: Generate Final Security Report

1. Switch to your **security agent** in the Copilot Chat dropdown
2. Ask it to perform a final security audit of the modernized PartsCatalogAPI, requesting:
    - A comparison to the original audit in `security-audit.md`
    - Which vulnerabilities were fixed during the migration
    - What security improvements were made
    - Any remaining concerns or recommendations
    - A before vs after security score
    - A comparison report suitable for your team
3. Save the response as `security-audit-after-migration.md`

**Expected improvements:**
- ✅ SQL injection vulnerabilities eliminated (EF Core parameterization)
- ✅ Async patterns implemented (better scalability)
- ✅ HTTPS redirection enabled
- ✅ Modern authentication configured
- ✅ Packages updated (no more CVEs from 2016!)
- ✅ Configuration secrets externalized

## Part 3: Modern Enhancements (Optional - if time permits)

Your migration plan is complete! If you have additional time, consider these optional modernizations with your agent.

### Step 1: Add Health Checks

Ask your modernization agent how to add health check endpoints for monitoring, including:
    - Basic liveness check at /health
    - Database connectivity check
    - Configuration in Program.cs
    - Response formatting options

Health checks are critical for containerized deployments and production monitoring.

### Step 2: Add Structured Logging

Ask your agent to configure structured logging, requesting:
- Console logging in structured format
- Correlation IDs for request tracking
- Different log levels for Development vs Production
- Required packages and Program.cs updates

Structured logging improves observability in production environments.

### Step 3: Consider Additional Modernizations

Ask your agent what other modern .NET 10 features could enhance this API, such as:
- Response caching strategies
- Rate limiting middleware
- OpenTelemetry for distributed tracing
- Minimal APIs as an alternative to controllers
- Native AOT compilation

Evaluate which features make sense for your production use case.

## Success Criteria

- [ ] Executed migration plan with agent guidance
- [ ] All controllers modernized (async, ControllerBase, IActionResult)
- [ ] EF6 migrated to EF Core 9
- [ ] Configuration moved from Web.config to appsettings.json
- [ ] SQL injection vulnerabilities fixed and verified
- [ ] Application builds without errors
- [ ] Application runs and serves requests
- [ ] Swagger/OpenAPI documentation accessible
- [ ] All endpoints tested and working
- [ ] Security audit shows significant improvement

## Troubleshooting

**Build errors about missing packages?**
```bash
dotnet restore
```
If issues persist, check package versions and compatibility.

**Runtime errors about configuration?**
- Verify appsettings.json syntax (valid JSON)
- Check connection string format for EF Core
- Ensure `IConfiguration` is registered and injected properly
- Confirm configuration file is set to "Copy to Output Directory"

**Database connection errors?**
- Update connection string to match your SQL Server instance
- For EF Core, connection string format may differ slightly from EF6
- Ensure SQL Server is running and accessible
- Consider running: `dotnet ef database update` to ensure schema is current

**Controller routing issues?**
- Verify `app.MapControllers()` is in Program.cs
- Check controller routing attributes match ASP.NET Core syntax
- Ensure controllers inherit from `ControllerBase` and have `[ApiController]`

**Authentication not working?**
- Verify JWT configuration in appsettings.json
- Check middleware order: `UseAuthentication()` BEFORE `UseAuthorization()`
- Test with a valid JWT token using tools like Postman
- Check that `AddAuthentication` and `AddJwtBearer` are configured

**Swagger not appearing?**
- Ensure Swashbuckle.AspNetCore package is referenced
- Check that `builder.Services.AddSwaggerGen()` is called
- Verify `app.UseSwagger()` and `app.UseSwaggerUI()` are in pipeline
- Only enable in Development: `if (app.Environment.IsDevelopment())`

## Reflection Questions

1. How many times did your agents reference earlier context or maintain consistency?
2. What part of the migration would have taken the longest without agent guidance?
3. How did having both modernization and security agents improve the outcome?
4. What additional patterns or knowledge would you add to your skill for future migrations?
5. How would you apply this agent-assisted approach on your real-world projects?
