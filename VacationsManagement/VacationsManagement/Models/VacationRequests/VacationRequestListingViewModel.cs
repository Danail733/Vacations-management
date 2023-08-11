using VacationsManagement.Enumerations;

namespace VacationsManagement.Models.VacationRequests
{
    public class VacationRequestListingViewModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string EmpoyeeName { get; set; }

        public string Status { get; set; }
    }
}
