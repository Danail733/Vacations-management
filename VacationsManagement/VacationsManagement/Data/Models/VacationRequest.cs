namespace VacationsManagement.Data.Models
{
    public class VacationRequest
    {
        public int Id { get; set; }

        public string RequestorId { get; set; }

        public Employee Requestor { get; set; }

        public string ReviewerId { get; set; }

        public Employee Reviewer { get; set; }

        public int StatusId { get; set; }

        public VacationStatus Status  { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DaysRequested { get; set; }

        public string? RejectReason { get; set; }
    }
}
