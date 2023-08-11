namespace VacationsManagement.Models.VacationRequests
{
    public class VacationRequestListingViewModel
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string EmpoyeeName { get; set; }

        public string Status { get; set; }

        public string RejectReason { get; set; }

        public int Vacationdays { get; set; }
    }
}
