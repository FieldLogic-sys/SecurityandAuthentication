# UserAuthInMemoryApp: Identity Commissioning Lab

## 🛠 Project Overview
Implementation of a mission-critical Authentication and Registration system using **ASP.NET Core 9.0** and **Identity EntityFrameworkCore**. This project utilizes an In-Memory database for rapid prototyping and logic verification.

## ⚡ Technical Stack
* **Framework:** .NET 9.0 (MVC)
* **Identity:** Microsoft.AspNetCore.Identity v9.0.0
* **Database:** Entity Framework Core (In-Memory Provider)
* **Hardware Interface:** Lenovo Yoga 9i / Windows 11 Pro Insider

## 🏗 Operational Logic: IActionResult vs. Task<IActionResult>
To maintain "Operational Integrity," this project distinguishes between manual and power-tool operations:
* **`IActionResult`**: Used for `[HttpGet]` actions. Instantaneous delivery of Views (Manual).
* **`async Task<IActionResult>`**: Used for `[HttpPost]` actions. Utilizes `await` to handle database writes and password hashing without blocking the server thread (Power Tool).



## 🔧 Installation & Setup
1. **Initialize Project:**
   ```bash
   dotnet new mvc -n UserAuthInMemoryApp
   cd UserAuthInMemoryApp
   ```
2. **Add Commissioned Packages:**
   ```bash
   dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
   dotnet add package Microsoft.AspNetCore.Identity.UI --version 9.0.*
   dotnet add package Microsoft.EntityFrameworkCore.InMemory
   ```

## 🔐 Implementation Details
### 1. Data Integrity (Models)
ViewModels utilize the `null!` operator to satisfy .NET 9 strict null-checking. The `RegisterViewModel` includes a required `ConfirmPassword` field to satisfy the validation contract.

### 2. The Actuator (AccountController)
Utilizes **Dependency Injection** via the constructor to access `UserManager` and `SignInManager`.

### 3. Middleware Configuration
The `Program.cs` pipeline is ordered for security:
`UseRouting` ➔ `UseAuthentication` ➔ `UseAuthorization` ➔ `MapControllerRoute`.

## ⚠️ Known Behaviors (In-Memory)
* **Volatility:** All user data is stored in volatile RAM. Restarting the application via `dotnet watch` or `Ctrl+C` will wipe the user registry.
* **Security:** Password hashing is handled automatically by the Identity engine using PBKDF2.