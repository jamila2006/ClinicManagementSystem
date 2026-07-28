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


## Checkpoint 1: User entity + şifrənin hash-lənməsi
- AppUser sinifi ASP.NET Core Identity-nin IdentityUser sinifindən miras alır,
  bu da Id, Email, PasswordHash sahələrini hazır təmin edir.
- Şifrə hash-lənməsi BCrypt əvəzinə ASP.NET Identity-nin daxili
  PasswordHasher (PBKDF2 alqoritmi) ilə həyata keçirilir — eyni məqsədi
  daşıyan, sənaye standartı bir alternativdir.
- Rollar layihənin domeninə uyğun olaraq ADMIN, DOCTOR, PATIENT seçilib
  (ümumi USER əvəzinə), çünki klinika idarəetmə sistemində bu, daha
  dəqiq səlahiyyət ayrımı təmin edir.
- Rollar tətbiq başlayanda RoleSeeder vasitəsilə avtomatik bazaya yazılır.
- AppUser, DoctorId/PatientId (nullable foreign key) sahələri ilə mövcud
  Doctor və Patient qeydlərinə bağlanır — login hesabı ilə klinika
  məlumatı arasında əlaqə qurulur.

## Checkpoint 2: Qeydiyyat + Giriş endpoint-ləri, JWT qaytarılması
- POST /api/auth/register — email, parol, rol (ADMIN/DOCTOR/PATIENT) qəbul edir.
  DOCTOR/PATIENT üçün mövcud Doctor/Patient qeydinə bağlanır (DoctorId/PatientId),
  hər qeyd yalnız bir hesaba bağlana bilər.
- POST /api/auth/login — email/parolu yoxlayır, uğurlu olarsa JWT token qaytarır.
- Şifrələr ASP.NET Identity-nin UserManager.CreateAsync() vasitəsilə avtomatik hash olunur.
- JWT secret appsettings.Development.json-da saxlanılır (Git-ə commit olunmur,
  appsettings.json-da yalnız placeholder var).