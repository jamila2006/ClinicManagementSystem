namespace ClinicManagementSystem.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly string _uploadsPath;
        private const long MaxFileSizeBytes = 2 * 1024 * 1024; // 2 MB

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png"
        };

        private static readonly Dictionary<string, byte[]> FileSignatures = new()
        {
            { ".jpg",  new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".png",  new byte[] { 0x89, 0x50, 0x4E, 0x47 } }
        };

        public FileService(IWebHostEnvironment env)
        {
            _uploadsPath = Path.Combine(env.ContentRootPath, "Uploads", "DoctorPhotos");
            Directory.CreateDirectory(_uploadsPath); // qovluq yoxdursa yaradır
        }

        public async Task<string> SaveDoctorPhotoAsync(int doctorId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Fayl boşdur.");

            if (file.Length > MaxFileSizeBytes)
                throw new ArgumentException($"Fayl ölçüsü {MaxFileSizeBytes / 1024 / 1024}MB-dan böyük ola bilməz.");

            var extension = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException("Yalnız .jpg, .jpeg, .png fayllarına icazə verilir.");

            if (!await IsValidFileSignatureAsync(file, extension))
                throw new ArgumentException("Faylın həqiqi məzmunu bəyan edilən tipə uyğun deyil.");

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadsPath, uniqueFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        public async Task<(byte[] Content, string ContentType, string FileName)?> GetDoctorPhotoAsync(string fileName)
        {
            var filePath = Path.Combine(_uploadsPath, fileName);

            if (!File.Exists(filePath))
                return null;

            var content = await File.ReadAllBytesAsync(filePath);
            var contentType = Path.GetExtension(fileName).ToLower() switch
            {
                ".png" => "image/png",
                _ => "image/jpeg"
            };

            return (content, contentType, fileName);
        }

        public void DeleteDoctorPhotoAsync(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            var filePath = Path.Combine(_uploadsPath, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        // Magic bytes yoxlaması — faylın İLK BYTE-LARINI oxuyur, uzantıya güvənmir
        private static async Task<bool> IsValidFileSignatureAsync(IFormFile file, string extension)
        {
            if (!FileSignatures.TryGetValue(extension, out var signature))
                return false;

            using var stream = file.OpenReadStream();
            var buffer = new byte[signature.Length];
            var bytesRead = await stream.ReadAsync(buffer, 0, signature.Length);

            if (bytesRead < signature.Length)
                return false;

            return buffer.SequenceEqual(signature);
        }
    }
}