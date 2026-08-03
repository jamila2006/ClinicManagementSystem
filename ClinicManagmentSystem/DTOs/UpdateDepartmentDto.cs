using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.DTOs
{
    public class UpdateDepartmentDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
