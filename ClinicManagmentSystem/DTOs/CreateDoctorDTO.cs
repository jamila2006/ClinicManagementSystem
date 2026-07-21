using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.DTOs
{
    public class CreateDoctorDTO
    {
        [Required, MaxLength(50)]
        public string FirstName{ get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email{ get; set; }
        [Phone]
        public string PhoneNumber{ get; set; }
        [Range(0,60)]
        public int ExperienceYears{ get; set; }
        [Range(1, 999999, ErrorMessage = "Valid DepartmentId is required")]
        public int DepartmentId{ get; set; }
    }
}
