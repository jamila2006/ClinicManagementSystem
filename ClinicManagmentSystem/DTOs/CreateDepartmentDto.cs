using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.DTOs
{
    public class CreateDepartmentDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
