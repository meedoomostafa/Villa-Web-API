# Villa Web Project

*Last indexed: 20 November 2025 (d876fa)*

---

## Overview

Villa Web is a multi-layered villa booking management platform. It consists of:

* **Backend API (`AppWebApi`)**: Handles all business logic, data access, and authentication.
* **Frontend MVC (`VillaWeb`)**: Provides role-based user interfaces for Customers and Companies.
* **Services (`AppService`)**: Implements business logic like CompanyService, VillaService.
* **Repositories (`AppRepository`)**: Data access with Repository pattern and Unit of Work.
* **Shared Models & DTOs (`AppModels`)**: Defines data transfer objects for API communication.

The project follows a clean architecture with separation of concerns and cross-cutting patterns like DI, logging, async operations, and AutoMapper.

---

## Architecture

### Multi-Project Structure

| Project       | Responsibility                   |
| ------------- | -------------------------------- |
| VillaWeb      | Frontend MVC application         |
| AppWebApi     | Backend REST API                 |
| AppService    | Business logic (Services)        |
| AppRepository | Data access layer (Repositories) |
| AppModels     | Shared domain models and DTOs    |

### Cross-Cutting Patterns

* Dependency Injection
* Repository + Unit of Work
* DTO Mapping via AutoMapper
* Async/Await for all API calls
* Centralized error handling
* Logging

---

## Backend (`AppWebApi`)

### Controllers

* **CompanyController**: CRUD operations for companies and villas.
* **VillaController**: Villa management operations.
* **BookingController**: Booking management endpoints.

### Services (`AppService`)

* **CompanyService**: Company dashboard, profile, villa management.
* **VillaService**: Villa creation, updates, retrieval.
* **BookingService**: Booking logic and status management.

### Data Access (`AppRepository`)

* EF Core with async methods.
* Repository pattern + Unit of Work for transaction handling.
* `CompanyRepository`, `VillaRepository` for database interaction.

---

## Frontend MVC (`VillaWeb`)

### Areas

* **Customer Area**: Browse villas, manage profile, view bookings.
* **Company Area**: Dashboard, villa management, booking monitoring.
* **Account Management**: Login, registration, role-based access.

### Services

* UnitOfServices centralizes API calls.
* `BaseService` handles HTTP client, JWT injection, response deserialization.

### DTO Mapping

* AutoMapper handles mapping between API DTOs and ViewModels.

### Security

* Cookie-based authentication with JWT in HttpOnly cookies.
* Role-based authorization for areas.
* Anti-forgery tokens for state-changing operations.
* SameSite cookie policies.

---

## Request Flow

1. User sends request → MVC Controller receives.
2. Claims extracted → User ID from authentication claims.
3. UnitOfServices routes request → appropriate service.
4. HTTP client sends request to API.
5. API processes request → Service → Repository → DB.
6. Response mapped to DTO → returned to MVC controller.
7. Razor view renders data to user.

---

## Technologies

* .NET 9.0 (MVC & Web API)
* Entity Framework Core
* ASP.NET Identity
* AutoMapper
* SQL Server
* Newtonsoft.Json
* HTTP Client (IHttpClientFactory)
* Async/Await

---

## Build & Run Instructions

### Backend (`AppWebApi`)

1. Build the project:

```bash
dotnet build
```

2. Create `appsettings.Development.json` with DB and Service URLs.
3. Run migrations:

```bash
dotnet ef migrations add initialCreate
```

```bash
dotnet ef database update
```

4. Run the backend:

```bash
dotnet watch run
```

### Frontend (`VillaWeb`)

1. Configure API URL in `appsettings.json`:

```json
{
  "ServiceUrls": {
    "VillaAPI": "https://localhost:5001"
  }
}
```

2. Build & run the frontend:

```bash
dotnet build
```

```bash
dotnet watch run
```

3. Open browser → default route (Customer area).

---

## Current Work / TODO

* Full admin functionality (dashboard, villa statistics, booking analytics).
* Enhanced role management and reporting.
* Additional frontend improvements for Company area.

---

## Notes

* DeepWiki generated detailed analysis of project structure, controllers, services, DTOs, and request flow.
* This README combines that analysis with practical build/run instructions.

---

## License

Specify your license here (e.g., MIT).
