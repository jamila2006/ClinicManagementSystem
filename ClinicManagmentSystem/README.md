# Clinic Management System — REST API

ASP.NET Core Web API ilə hazırlanmış klinika idarəetmə sistemi. Doctor, Patient, Department və Appointment resursları üçün tam CRUD əməliyyatları, qatlı arxitektura (Controller → Service → Repository), pagination/sorting, validasiya, mərkəzləşdirilmiş exception handling və Swagger sənədləşdirməsi daxildir.

## Texnologiyalar

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- ASP.NET Core Identity
- Swagger / OpenAPI (Swashbuckle)
- xUnit + Moq (unit testlər)

## Quraşdırma addımları

### 1. Repository-ni klonlayın

git clone https://github.com/jamila2006/ClinicManagementSystem.git
cd ClinicManagementSystem

### 2. Verilənlər bazası bağlantısını konfiqurasiya edin

`appsettings.json` faylında `ConnectionStrings.Default` dəyərini öz SQL Server instansiyanıza uyğun tənzimləyin:

{
  "ConnectionStrings": {
    "Default": "Server=YOUR_SERVER_NAME; Database=ClinicManagementSystemDb; Trusted_Connection=True; TrustServerCertificate=True;"
  }
}

### 3. Miqrasiyaları tətbiq edin

dotnet ef database update

### 4. Layihəni işə salın

dotnet run

### 5. Swagger UI-a keçin

https://localhost:7076/swagger/index.html

## Testlərin işə salınması

cd ClinicManagementSystem.Tests
dotnet test

## Xüsusiyyətlər

- Pagination və sorting bütün siyahı endpoint-lərində
- Input validasiyası (Required, MaxLength, EmailAddress, Range)
- Mərkəzləşdirilmiş exception handling (ExceptionMiddleware)
- Swagger/OpenAPI sənədləşdirməsi
- Unit testlər (DoctorService, PatientService)