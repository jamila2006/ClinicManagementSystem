namespace ClinicManagementSystem.Services
{
    public interface IFileService
    {
        Task<string> SaveDoctorPhotoAsync(int doctorId, IFormFile file);
        Task<(byte[] Content, string ContentType, string FileName)?> GetDoctorPhotoAsync(string fileName);
        void DeleteDoctorPhotoAsync(string? fileName);
    }
}
