namespace ClinicManagementSystem.Models
{
    public class Patient
    {
        public int Id{ get; set; }
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public DateTime BirthDate{ get; set; }
        public string Gender{ get; set; }
        public string Email{ get; set; }
        public string PhoneNumber{ get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
