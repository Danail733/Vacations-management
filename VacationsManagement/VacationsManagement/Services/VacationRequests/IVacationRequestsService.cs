using VacationsManagement.Models.VacationRequests;

namespace VacationsManagement.Services.VacationRequests
{
    public interface IVacationRequestsService
    {
        public int SubmitVacationRequest(DateTime startDate, DateTime endDate, string userId);

        public IEnumerable<VacationRequestListingViewModel> GetAllManagerRequests(string managerId);

        public int ApproveVacationRequest(int requestId);

        public int RejectVacationRequest(int requestId, string rejectReason);

        public IEnumerable<VacationRequestListingViewModel> GetMyRequests(string userId);
    }
}
