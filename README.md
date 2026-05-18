

# 🩺 PCOS Analyzer API

**Backend .NET 8 Web API for Polycystic Ovary Syndrome Diagnosis**

This is a robust, scalable backend system built with **ASP.NET Core** and **Clean Architecture** principles. The system provides automated diagnostics and management tools for PCOS.

## 🚀 Key Features

  * **Automated Diagnosis:** Core logic for PCOS detection based on medical parameters.
  * **Identity & Security:** Secure authentication and authorization using ASP.NET Core Identity.
  * **Scalable Architecture:** Implemented using **Clean Architecture** (Separation of Concerns).
  * **Database Management:** Efficient data handling with **Entity Framework Core** and **Dapper**.
  * **Real-time Support:** Integrated **SignalR** for real-time notifications (if applicable).
  * **Optimized Performance:** Caching mechanism using **Redis**.

## 🏗️ Tech Stack

  * **Framework:** .NET 8 / ASP.NET Core Web API
  * **Architecture:** Clean Architecture (Domain, Application, Infrastructure, API Layers)
  * **Database:** SQL Server
  * **ORM:** Entity Framework Core & Dapper
  * **Communication:** SignalR
  * **Caching:** Redis
  * **Documentation:** Swagger UI

## 📂 Project Structure

```text
src/
├── PcosAnalyzer.API          # Entry point and Controllers
├── Application/              # Business Logic and Interfaces
├── Domain/                   # Entities and Domain Models
├── Infrastructure/           # External Services (Mail, Identity)
└── Persistence/              # Data Access Layer (EF Core, Migrations)
```

## 🛠️ How to Run Locally

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/your-username/PcosAnalyzer.git
    ```
2.  **Update Connection String:**
    Modify `appsettings.json` in `PcosAnalyzer.API` with your local SQL Server details.
3.  **Apply Migrations:**
    ```bash
    dotnet ef database update
    ```
4.  **Run the Project:**
    ```bash
    dotnet run --project PcosAnalyzer.API
    ```

