namespace ClinicManagementSystem.DTOs
{
    public class CreateDoctorDTO
    {
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string Email{ get; set; }
        public string PhoneNumber{ get; set; }
        public int ExperienceYears{ get; set; }
        public int DepartmentId{ get; set; }
    }
}
