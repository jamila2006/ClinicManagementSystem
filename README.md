# Clinic Management System

ASP.NET Core Web API ile hazirlanmis klinika idareetme sistemi.

## Texnologiyalar
- ASP.NET Core Web API (.NET)
- Entity Framework Core + SQL Server
- ASP.NET Core Identity + JWT
- IMemoryCache
- BackgroundService
- Swagger / OpenAPI

## Quraşdirma
1. git clone https://github.com/jamila2006/ClinicManagementSystem.git
2. cd ClinicManagementSystem/ClinicManagmentSystem
3. dotnet restore
4. appsettings.Development.json-da connection string-i uygunlashdir
5. dotnet user-secrets init
6. dotnet user-secrets set "Jwt:Key" "OWN_MIN_32_CHAR_SECRET_KEY"
7. dotnet ef database update
8. dotnet run
9. Swagger: https://localhost:7076/swagger

## Autentifikasiya
1. POST /api/auth/register ile qeydiyyat
2. POST /api/auth/login ile JWT token al
3. Swagger-de Authorize dumesine "Bearer {token}" yaz
