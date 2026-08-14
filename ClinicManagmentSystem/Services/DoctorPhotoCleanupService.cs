using ClinicManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Services.Implementations
{
    public class DoctorPhotoCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DoctorPhotoCleanupService> _logger;
        private readonly string _uploadsPath;
        private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

        public DoctorPhotoCleanupService(
            IServiceScopeFactory scopeFactory,
            ILogger<DoctorPhotoCleanupService> logger,
            IWebHostEnvironment env)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _uploadsPath = Path.Combine(env.ContentRootPath, "Uploads", "DoctorPhotos");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Interval);

            // Tətbiq işə düşən kimi bir dəfə də icra et (24 saat gözləmədən)
            await CleanupOrphanedPhotosAsync(stoppingToken);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await CleanupOrphanedPhotosAsync(stoppingToken);
            }
        }

        private async Task CleanupOrphanedPhotosAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (!Directory.Exists(_uploadsPath)) return;

                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var referencedFileNames = await context.Doctors
                    .Where(d => d.PhotoUrl != null)
                    .Select(d => d.PhotoUrl!)
                    .ToListAsync(cancellationToken);

                var referencedSet = new HashSet<string>(referencedFileNames, StringComparer.OrdinalIgnoreCase);

                var filesOnDisk = Directory.GetFiles(_uploadsPath).Select(Path.GetFileName);

                int deletedCount = 0;
                foreach (var fileName in filesOnDisk)
                {
                    if (fileName != null && !referencedSet.Contains(fileName))
                    {
                        File.Delete(Path.Combine(_uploadsPath, fileName));
                        deletedCount++;
                    }
                }

                _logger.LogInformation("Doctor photo cleanup: {Count} yetim fayl silindi", deletedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Doctor photo cleanup zamanı xəta baş verdi");
            }
        }
    }
}