using VacationsManagement.Models.VacationRequests;

namespace VacationsManagement.Services
{
    public interface IVacationRequestsService
    {
        public int SubmitVacationRequest(DateTime startDate, DateTime endDate, string userId);

        public IEnumerable<VacationRequestListingViewModel> GetAllManagerRequests(string managerId);
    }
}
