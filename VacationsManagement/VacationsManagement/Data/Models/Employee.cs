using Microsoft.AspNetCore.Identity;

namespace VacationsManagement.Data.Models
{
    public class Employee : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ManagerId { get; set; }

        public Employee Manager { get; set; }

        public int VacationDays { get; set; }

        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

        public IEnumerable<VacationRequest> RequestVacations { get; set; } = new List<VacationRequest>();

        public IEnumerable<VacationRequest> RequestsToReview { get; set; } = new List<VacationRequest>();
    }
}
