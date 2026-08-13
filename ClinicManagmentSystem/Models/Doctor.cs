namespace ClinicManagementSystem.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ExperienceYears { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public string? PhotoUrl { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
