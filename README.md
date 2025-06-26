# Villa Web API - Complete Solution

A comprehensive Villa Management System built with ASP.NET Core, featuring a RESTful Web API and an MVC web client for complete villa management functionality. This solution demonstrates modern .NET development practices with clean architecture and JWT authentication.

## 🏗️ Solution Architecture

### **VillaWebApi** - RESTful Web API
- **Framework**: ASP.NET Core Web API (.NET 8)
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **ORM**: Entity Framework Core with LINQ
- **Architecture**: Clean Architecture with Repository Pattern

### **VillaWeb** - MVC Web Client
- **Framework**: ASP.NET Core MVC
- **Purpose**: Consumes the VillaWebApi
- **Dependencies**: VillaWebUtility for shared utilities

### **VillaWebUtility** - Shared Library
- **Purpose**: Common utilities and DTOs shared between API and MVC projects

## 🚀 Features

- **Villa Management**: Full CRUD operations for villa entities
- **User Authentication & Authorization**: JWT-based security implementation
- **Web Client Interface**: User-friendly MVC interface for villa management
- **RESTful API Design**: Following REST architectural principles
- **Database Integration**: SQL Server with Entity Framework Core
- **Error Handling**: Comprehensive error handling and logging
- **Data Validation**: Model validation and business rule enforcement

## 📋 Prerequisites

Before running this project, make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Full version)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## ⚙️ Configuration

### 1. API Configuration (`VillaWebApi/appsettings.json`)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "CS": "Server=(localdb)\\MSSQLLocalDB;Database=VillaDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "JWT": {
    "Key": "Your-Super-Secret-Key-That-Is-At-Least-32-Characters-Long",
    "Issuer": "https://localhost:7207",
    "Audience": "https://localhost:7291",
    "LifeTime": 30
  }
}
```

### 2. MVC Client Configuration (`VillaWeb/appsettings.json`)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ServiceUrls": {
    "VillaAPI": "https://localhost:7207"
  }
}
```

## 🛠️ Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/meedoomostafa/Villa-Web-API.git
cd Villa-Web-API
```

### 2. Update Configuration Files

Update the `appsettings.json` files in both projects with your developer-specific data:
- Database connection string
- JWT secret key (ensure it's at least 32 characters)
- Service URLs

### 3. Clean and Build Solution

```bash
dotnet clean
dotnet build
```

### 4. Setup Database

Navigate to the API project and update the database:

```bash
cd VillaWebApi
dotnet ef database update
```

If migrations don't exist, create them first:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the API

From the `VillaWebApi` directory:

```bash
dotnet watch --launch-profile https run
```

The API will be available at:
- **HTTPS**: `https://localhost:7207`
- **Swagger UI**: `https://localhost:7207/swagger`

### 6. Run the MVC Client (Optional)

From the `VillaWeb` directory:

```bash
dotnet run
```

## 📁 Project Structure

```
Villa-Web-API/
├── VillaWebApi/              # 🔥 Main API Project
│   ├── Controllers/          # API Controllers
│   ├── Models/              # Data models and entities
│   ├── Data/                # DbContext and configurations
│   ├── Services/            # Business logic services
│   ├── Repositories/        # Data access layer
│   ├── Middlewares/         # Custom middlewares
│   ├── Extensions/          # Extension methods
│   ├── appsettings.json     # API configuration
│   └── Program.cs           # API entry point
│
├── VillaWeb/                # 🌐 MVC Web Client
│   ├── Controllers/         # MVC Controllers
│   ├── Views/              # Razor views
│   ├── Models/             # View models
│   ├── Services/           # API client services
│   ├── appsettings.json    # MVC configuration
│   └── Program.cs          # MVC entry point
│
└── VillaWebUtility/        # 📚 Shared Library
    ├── DTOs/               # Data Transfer Objects
    ├── Helpers/            # Utility classes
    └── Constants/          # Shared constants
```

## 📚 API Documentation

Once the API is running, access the Swagger documentation at:
- `https://localhost:7207/swagger`

### Main API Endpoints

| Method | Endpoint | Description | Authentication |
|--------|----------|-------------|----------------|
| GET | `/api/villas` | Get all villas | ✅ Required |
| GET | `/api/villas/{id}` | Get villa by ID | ✅ Required |
| POST | `/api/villas` | Create new villa | ✅ Required |
| PUT | `/api/villas/{id}` | Update existing villa | ✅ Required |
| DELETE | `/api/villas/{id}` | Delete villa | ✅ Required |
| POST | `/api/auth/register` | Register new user | ❌ Public |
| POST | `/api/auth/login` | User login | ❌ Public |

## 🔧 Development Workflow

### Standard Build Process

```bash
# Navigate to solution root
cd Villa-Web-API

# Clean previous builds
dotnet clean

# Build the entire solution
dotnet build

# Navigate to API project for database operations
cd VillaWebApi

# Update database with latest migrations
dotnet ef database update

# Run API with hot reload and HTTPS profile
dotnet watch --launch-profile https run
```

### Database Operations

```bash
# From VillaWebApi directory

# Add new migration
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# Generate SQL script
dotnet ef migrations script

# Drop database (use with caution)
dotnet ef database drop
```

### Running Different Projects

```bash
# Run API only (from VillaWebApi directory)
dotnet watch --launch-profile https run

# Run MVC client only (from VillaWeb directory)
dotnet run

# Run specific project from solution root
dotnet run --project VillaWebApi
dotnet run --project VillaWeb
```

## 🔐 Authentication Flow

This solution uses JWT authentication:

### For API Direct Access:
1. **Register**: `POST /api/auth/register`
2. **Login**: `POST /api/auth/login` → Receive JWT token
3. **Use Token**: Include in Authorization header: `Bearer <your-token>`

### For MVC Client:
The MVC client handles authentication flow automatically and stores tokens securely for API communication.

## 🌐 MVC Web Client Features

The **VillaWeb** project provides:
- User-friendly interface for villa management
- Authentication pages (Login/Register)
- Villa listing and details pages
- Create/Edit villa forms
- Responsive design
- Automatic API communication through **VillaWebUtility**

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection Issues**
   ```bash
   # Check if SQL Server is running
   # Verify connection string in VillaWebApi/appsettings.json
   # Run from VillaWebApi directory:
   dotnet ef database update
   ```

2. **JWT Token Issues**
   - Ensure JWT Key is at least 32 characters
   - Check token expiration (LifeTime setting)
   - Verify Issuer and Audience URLs match your setup

3. **API Communication Issues**
   - Verify ServiceUrls in VillaWeb/appsettings.json
   - Ensure VillaWebApi is running before starting VillaWeb
   - Check CORS settings if accessing from different ports

4. **Build Issues**
   ```bash
   # Clean and rebuild entire solution
   dotnet clean
   dotnet build
   
   # Check for missing packages
   dotnet restore
   ```

## 🚀 Deployment Considerations

### API Deployment
- Configure production connection string
- Set up environment-specific JWT settings
- Enable HTTPS in production
- Configure CORS for your domain

### MVC Client Deployment
- Update ServiceUrls to point to production API
- Configure production logging
- Set up static file handling

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m 'Add some feature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

## 👨‍💻 Developer

**Mostafa Meedo**
- **GitHub**: [@meedoomostafa](https://github.com/meedoomostafa)
- **University**: Zagazig University - Faculty of Computers and Information
- **Specialization**: Computer Science
- **Skills**: C++, C#, SQL Server, HTML, CSS, LINQ, EF Core, Git & GitHub, OOP, Data Structures, Algorithms, Design Principles
- **Interests**: Problem Solving, Competitive Programming, AI, Deep-dive Technical Learning

## 🛡️ Security Best Practices

- Store sensitive configuration in environment variables
- Use strong JWT secret keys (32+ characters)
- Implement proper input validation
- Use HTTPS in production
- Regularly update dependencies
- Implement rate limiting for API endpoints

## 📊 Performance Tips

- Use async/await for all database operations
- Implement caching strategies
- Optimize database queries with proper indexing
- Use pagination for large datasets
- Consider implementing API versioning for future updates

---

**🏡 Building Amazing Villa Management Solutions with .NET! 🚀**