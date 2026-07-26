using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.Models
{
    public class AppUser : IdentityUser
    {
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public int? PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}

