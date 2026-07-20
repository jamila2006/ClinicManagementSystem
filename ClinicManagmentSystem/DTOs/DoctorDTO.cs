using System.Security.Principal;

namespace ClinicManagementSystem.DTOs
{
    public class DoctorDTO
    {
        public int Id{ get; set; }
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public string Email{ get; set; }
        public string PhoneNumber{ get; set; }
        public int ExperienceYears{ get; set; }
        public int DepartmentId{ get; set; }
        public string DepartmentName{ get; set; }
    }
}
