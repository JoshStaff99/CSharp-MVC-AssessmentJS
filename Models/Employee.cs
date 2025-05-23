using System.ComponentModel.DataAnnotations;

namespace CSharp_MVC_AssessmentJS.Models
{
    public class Employee 
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        [EmailAddress]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}