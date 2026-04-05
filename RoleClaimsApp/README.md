# RoleClaimsApp: ASP.NET Core 9.0 Identity & Authorization

**Status:** Commissioned (Inbetriebnahme)  
**Engineer:** Anthony Edward Aldea  
**Architecture:** Web API with In-Memory Identity  
**Core Motto:** "Control the memory, control the machine."

---

## ▣ Project Overview
This project demonstrates a production-standard implementation of **ASP.NET Core Identity**. Unlike standard labs that "mock" user data, this application utilizes a real **Middleware Pipeline** and a **Seeding Engine** to manage Role-Based Access Control (RBAC) and Claims-Based Authorization (ABAC).

### Key Features:
* **Decoupled Security:** Uses `[Authorize]` attributes to separate security logic from business logic.
* **Automatic Commissioning:** A `SeedIdentityAsync` task ensures the "Admin" role and "IT" claims are in RAM at startup.
* **Security+ Alignment:** Implements **Implicit Deny** via the Authorization middleware.

---

## ▣ Technical Stack
* **Runtime:** .NET 9.0 SDK
* **Persistence:** Entity Framework Core (In-Memory Provider)
* **Hardware Suite:** Lenovo Yoga 9i (12th Gen i7)
* **Packages:**
    * `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (v9.0.*)
    * `Microsoft.EntityFrameworkCore.InMemory` (v9.0.*)

---

## ▣ System Configuration (Program.cs)
The application pipeline is ordered strictly to ensure the "ID Badge" is checked before "Permissions" are evaluated:

1.  **AddDbContext**: Configures the `ApplicationDbContext` for In-Memory storage.
2.  **AddIdentity**: Registers `UserManager` and `RoleManager`.
3.  **AddAuthorization**: Defines custom policies (e.g., `RequireITDepartment`).
4.  **Middleware Order**: 
    * `UseRouting()`
    * `UseAuthentication()`
    * `UseAuthorization()`

---

## ▣ Execution & Testing

### 1. Build the Machine
```powershell
dotnet clean
dotnet build
dotnet run