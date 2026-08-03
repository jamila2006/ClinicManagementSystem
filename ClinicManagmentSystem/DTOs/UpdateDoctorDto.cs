using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.DTOs
{
    public class UpdateDoctorDto
    {
        [Required, MaxLength(50)]
        public string FirstName{ get; set; }
        [Required, MaxLength(50)]
        public string LastName{ get; set; }
        [Required, EmailAddress]
        public string Email{ get; set; }
        [Required, Phone]
        public string PhoneNumber{ get; set; }
        [Range(0,50)]
        public int ExperienceYears{ get; set; }
        [Range(1, 9999, ErrorMessage ="Valid DepartmentId is required.")]
        public int DepartmentId{ get; set; }
    }
}
