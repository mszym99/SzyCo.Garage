# SzyCo.Garage

A modern web-based vehicle garage management system built with IntelliTect Coalesce, ASP.NET Core 9.0, and Vue 3.

## Overview

SzyCo.Garage allows users to track and manage their personal vehicle collections with features including:
- 🚗 Vehicle tracking (make, model, year, color)
- 👥 User account management with role-based access control
- 🔒 Permission-based authorization (Admin, UserAdmin, ViewAuditLogs)
- 📜 Comprehensive audit logging of all data changes
- 🎨 Modern Material Design UI with Vuetify 3

## Technology Stack

### Backend
- **.NET 9.0** - Latest LTS framework
- **ASP.NET Core Identity** - Authentication and authorization  
- **Entity Framework Core 9.0** - ORM with SQL Server
- **IntelliTect Coalesce 5.3.6** - Code generation framework

### Frontend
- **Vue 3.5** - Progressive JavaScript framework
- **Vuetify 3.7** - Material Design component library
- **TypeScript 5.5** - Type-safe JavaScript
- **Vite 5.4** - Fast build tool with HMR

### Testing
- **xUnit** - Backend unit testing
- **Vitest 2.1** - Frontend unit testing
- **Vue Test Utils** - Component testing

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [Node.js 22.x](https://nodejs.org/) or later
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (Express, Developer, or full edition)
- A code editor (Visual Studio 2022, VS Code, or Rider recommended)

## Getting Started

### 1. Clone the Repository

\`\`\`bash
git clone https://github.com/mszym99/SzyCo.Garage.git
cd SzyCo.Garage
\`\`\`

### 2. Configure Database Connection

Update the connection string in \`SzyCo.Garage.Web/appsettings.json\`:

\`\`\`json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=mount-database;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
\`\`\`

For production, use environment variables or Azure Key Vault instead of hardcoding credentials.

### 3. Install Dependencies

**Backend (NuGet):**
\`\`\`bash
dotnet restore
\`\`\`

**Frontend (npm):**
\`\`\`bash
cd SzyCo.Garage.Web
npm ci
\`\`\`

### 4. Apply Database Migrations

\`\`\`bash
cd SzyCo.Garage.Web
dotnet ef database update --project ../SzyCo.Garage.Data
\`\`\`

This creates the database and applies all migrations.

### 5. Generate Coalesce Code

Coalesce generates TypeScript models, API clients, and controller code from your C# models:

\`\`\`bash
cd SzyCo.Garage.Web
dotnet coalesce
\`\`\`

> **Note:** This step is crucial after any model changes. Generated files have a \`.g.ts\` or \`.g.cs\` extension.

### 6. Build Frontend Assets

\`\`\`bash
cd SzyCo.Garage.Web
npm run build
\`\`\`

### 7. Run the Application

**Option A: Production Mode (ASP.NET Core only)**
\`\`\`bash
cd SzyCo.Garage.Web
dotnet run
\`\`\`
Navigate to: \`https://localhost:5001\`

**Option B: Development Mode (with Vite HMR)**

Terminal 1 - Backend:
\`\`\`bash
cd SzyCo.Garage.Web
dotnet run
\`\`\`

Terminal 2 - Frontend:
\`\`\`bash
cd SzyCo.Garage.Web
npm run dev
\`\`\`
Navigate to: \`http://localhost:5173\` (Vite dev server with hot module replacement)

### 8. Create Initial Admin User

On first run, you'll need to create an admin user. You can do this via:

1. Register a new account through the UI
2. Use SQL Server Management Studio to manually assign admin role
3. Or use EF Core migrations to seed an initial admin user (recommended for production)

Example SQL to promote a user to admin:
\`\`\`sql
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u
CROSS JOIN AspNetRoles r
WHERE u.UserName = 'your@email.com' AND r.Name = 'Admin'
\`\`\`

## Development Workflow

### Project Structure

\`\`\`
SzyCo.Garage/
├── SzyCo.Garage.Data/           # Domain models, EF Core context, data sources
│   ├── Models/                  # Entity models (Car, User, Role, etc.)
│   ├── Migrations/              # EF Core migrations
│   ├── Coalesce/                # Custom data sources and behaviors
│   └── Services/                # Business logic services
├── SzyCo.Garage.Data.Test/      # xUnit tests for data layer
├── SzyCo.Garage.Web/            # ASP.NET Core web application
│   ├── Api/Generated/           # Generated API controllers (DO NOT EDIT)
│   ├── Models/Generated/        # Generated DTOs (DO NOT EDIT)
│   ├── src/                     # Vue 3 frontend source
│   │   ├── components/          # Reusable Vue components
│   │   ├── views/               # Page-level components
│   │   ├── composables/         # Vue 3 composition utilities
│   │   ├── *.g.ts               # Generated TypeScript (DO NOT EDIT)
│   │   └── router.ts            # Vue Router configuration
│   ├── wwwroot/                 # Static files (index.html)
│   └── Program.cs               # ASP.NET Core entry point
├── coalesce.json                # Coalesce configuration
├── ASSESSMENT.md                # Repository health assessment
└── README.md                    # This file
\`\`\`

### Coalesce Code Generation

**What is Coalesce?**

Coalesce is a rapid development framework that generates:
- TypeScript/JavaScript client models and view models
- API endpoints and controllers
- Type-safe API clients
- Vue components (optional)

**When to regenerate:**

Run \`dotnet coalesce\` whenever you:
- Add or modify C# model classes
- Change model properties or relationships
- Add/modify \`[Coalesce]\` attributes
- Update data sources or behaviors

**Files to NEVER edit manually:**
- \`*.g.cs\` - Generated C# files
- \`*.g.ts\` - Generated TypeScript files
- \`Api/Generated/*\` - Generated API controllers

**Where to put custom code:**
- Partial classes (e.g., \`Car.cs\` can have a \`Car.Partial.cs\`)
- Custom data sources (inherit from \`StandardDataSource<T>\`)
- Custom behaviors (inherit from \`StandardBehaviors<T>\`)
- Vue components in \`src/components/\` and \`src/views/\`

### Common Development Tasks

**Add a new entity:**
1. Create model class in \`SzyCo.Garage.Data/Models/\`
2. Add \`[Coalesce]\` attribute
3. Add \`DbSet<YourEntity>\` to \`AppDbContext\`
4. Create migration: \`dotnet ef migrations add AddYourEntity --project SzyCo.Garage.Data\`
5. Run: \`dotnet coalesce\`
6. Apply migration: \`dotnet ef database update --project SzyCo.Garage.Data\`

**Modify existing entity:**
1. Update model class
2. Create migration: \`dotnet ef migrations add UpdateYourEntity --project SzyCo.Garage.Data\`
3. Run: \`dotnet coalesce\`
4. Apply migration: \`dotnet ef database update --project SzyCo.Garage.Data\`

**Add custom business logic:**
1. Create a custom data source or behavior in \`SzyCo.Garage.Data/Coalesce/\`
2. Or create a service class marked with \`[Service]\` attribute
3. Run: \`dotnet coalesce\` to generate API endpoints

**Lint and format code:**
\`\`\`bash
# Backend (automatic with build)
dotnet build

# Frontend
cd SzyCo.Garage.Web
npm run lint        # Check for issues
npm run lint:fix    # Auto-fix issues
\`\`\`

**Run tests:**
\`\`\`bash
# Backend
dotnet test

# Frontend
cd SzyCo.Garage.Web
npm test           # Interactive mode
npm run coverage   # With coverage report
\`\`\`

## Architecture Overview

### Data Flow

\`\`\`
C# Models (with [Coalesce] attributes)
    ↓
Coalesce Code Generator (dotnet coalesce)
    ↓
├─→ TypeScript Models/ViewModels (*.g.ts)
├─→ API Controllers (Api/Generated/*.g.cs)
└─→ API Clients (api-clients.g.ts)
    ↓
Vue 3 Components consume generated APIs
    ↓
Vuetify 3 UI renders data
\`\`\`

### Authentication & Authorization

1. **Cookie-based authentication** via ASP.NET Core Identity
2. **Custom claims**: Role, Email, UserId, UserName
3. **Security stamp validation**: Every 5 minutes
4. **Password policy**: 15-character minimum (NIST 800-63-4 compliant)
5. **Frontend auth**: \`user-service.ts\` maintains global auth state

**Permission System:**
- \`Admin\` - Application configuration (non-user management)
- \`UserAdmin\` - Create/manage users and roles
- \`ViewAuditLogs\` - Access audit log viewing

### Email confirmation (`EmailConfirmed`)

- The **Email Confirmed** column maps to ASP.NET Identity's \`AspNetUsers.EmailConfirmed\` flag.
- It is set to \`true\` only when the user successfully opens a valid confirmation link handled by \`/ConfirmEmail\`.
- Flow is implemented in:
  - \`SzyCo.Garage.Web/Pages/Register.cshtml.cs\` (sends confirmation request after registration)
  - \`SzyCo.Garage.Data/Auth/UserManagementService.cs\` (generates token + confirmation URL)
  - \`SzyCo.Garage.Web/Pages/ConfirmEmail.cshtml.cs\` (calls \`UserManager.ConfirmEmailAsync\`)

To make this operational outside development, configure \`Communication:Email\` in \`SzyCo.Garage.Web/appsettings.json\` (or environment variables).  
Use either:
- SMTP host configuration (\`SmtpHost\`, \`SmtpPort\`, optional credentials), or
- \`PickupDirectory\` for local/test mail-drop behavior.

Suggested follow-up issues:
- Enforce confirmed email at sign-in (\`SignIn.RequireConfirmedAccount = true\`) when rollout is ready.
- Add explicit admin UI action to resend confirmation from the user list.
- Add integration test coverage for end-to-end confirmation flow.

**Authorization checks:**
- Backend: \`[Edit(nameof(Permission.UserAdmin))]\` attributes on models
- Frontend: \`can(Permission.UserAdmin)\` helper in components

### Database Schema

**Core Tables:**
- \`AspNetUsers\` - User accounts (extended with FullName)
- \`AspNetRoles\` - Roles with Permissions field
- \`Cars\` - Vehicle records linked to users
- \`AuditLogs\` - Change tracking for all entities
- \`Widgets\` - Sample entity (for demo/learning purposes)

**Relationships:**
- User 1:N Cars (one user, many cars)
- User N:M Roles (via AspNetUserRoles)

## CI/CD

The project includes Azure Pipelines configuration that:

1. ✅ Installs .NET 9.0 and Node.js 22.x
2. ✅ Restores dependencies
3. ✅ Verifies Coalesce code generation is up-to-date
4. ✅ Runs ESLint
5. ✅ Builds frontend assets
6. ✅ Builds .NET solution (warnings as errors)
7. ✅ Runs tests
8. ✅ Publishes artifacts

## Troubleshooting

### "Coalesce code is out of date"

Run:
\`\`\`bash
cd SzyCo.Garage.Web
dotnet coalesce
\`\`\`

### Migration fails with "pending migrations"

Check current migration status:
\`\`\`bash
dotnet ef migrations list --project SzyCo.Garage.Data
\`\`\`

Apply pending migrations:
\`\`\`bash
dotnet ef database update --project SzyCo.Garage.Data
\`\`\`

### Frontend build fails

Clear node_modules and reinstall:
\`\`\`bash
cd SzyCo.Garage.Web
rm -rf node_modules package-lock.json
npm install
npm run build
\`\`\`

### Database connection fails

Verify SQL Server is running and connection string is correct:
\`\`\`bash
dotnet ef database update --project SzyCo.Garage.Data --verbose
\`\`\`

### npm security vulnerabilities

Run:
\`\`\`bash
cd SzyCo.Garage.Web
npm audit fix
\`\`\`

For breaking changes:
\`\`\`bash
npm audit fix --force  # Use with caution
\`\`\`

## Contributing

1. Fork the repository
2. Create a feature branch (\`git checkout -b feature/AmazingFeature\`)
3. Make your changes
4. Run tests: \`dotnet test && npm test\`
5. Commit your changes (\`git commit -m 'Add some AmazingFeature'\`)
6. Push to the branch (\`git push origin feature/AmazingFeature\`)
7. Open a Pull Request

## Resources

- [IntelliTect Coalesce Documentation](https://intellitect.github.io/Coalesce/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Vue 3 Documentation](https://v3.vuejs.org/)
- [Vuetify 3 Documentation](https://vuetifyjs.com/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or contributions, please open an issue on GitHub.
