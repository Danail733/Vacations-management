namespace VacationsManagement.Data.Models
{
    public class VacationStatus
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public IEnumerable<VacationRequest> VacationRequests { get; set; } = new List<VacationRequest>();
    }
}
