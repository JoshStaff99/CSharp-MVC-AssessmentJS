using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CSharp_MVC_AssessmentJS.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? LogoPath { get; set; }

        [Url]
        public string? Website { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}