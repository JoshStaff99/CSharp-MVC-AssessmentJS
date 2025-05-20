using System.Collections.Generic;

namespace CSharp_MVC_AssessmentJS.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalCompanies { get; set; }
        public int TotalEmployees { get; set; }
        public List<Company> RecentCompanies { get; set; }
        public List<Employee> RecentEmployees { get; set; }
    }
}