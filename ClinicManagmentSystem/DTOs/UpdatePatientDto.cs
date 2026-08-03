using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.DTOs
{
    public class UpdatePatientDto
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
