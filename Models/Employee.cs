using System.ComponentModel.DataAnnotations;

namespace CSharp_MVC_AssessmentJS.Models
{
    public class Employee 
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}