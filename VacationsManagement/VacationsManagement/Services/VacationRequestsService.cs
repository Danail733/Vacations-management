using VacationsManagement.Data;
using VacationsManagement.Data.Models;
using VacationsManagement.Enumerations;
using VacationsManagement.Models.VacationRequests;
using VacationsManagement.Services.Users;

namespace VacationsManagement.Services
{
    public class VacationRequestsService : IVacationRequestsService
    {
        private readonly IUserService _userService;
        private readonly VacationManagementDbContext _context;

        public VacationRequestsService(IUserService userService, VacationManagementDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public int SubmitVacationRequest(DateTime startDate, DateTime endDate, string userId)
        {
            var countOfRequestedDays = endDate.Day - startDate.Day + 1;
            var remainingVacationDays = _userService.GetVacationDaysByUserId(userId);
            var managerId = _userService.GetManagerIdByUserId(userId);

            if(remainingVacationDays == 0 || managerId == null)
            {
                return -1;
            }

            var validationResult = ValidateVacation(startDate, endDate, countOfRequestedDays, remainingVacationDays, userId);

            if(validationResult == -1)
            {
                return -1;
            }
              
            var vacationRequest = new VacationRequest
            {
                RequestorId = userId,
                ReviewerId = managerId,
                StatusId = (int)VacationRequestStatus.Pending,
                StartDate = startDate,
                EndDate = endDate,
                DaysRequested = countOfRequestedDays
            };

            _context.VacationRequests.Add(vacationRequest);
            _context.SaveChanges();

            _userService.UpdateVacationDaysByUserId(userId, countOfRequestedDays);

            return 0;
        }

        public IEnumerable<VacationRequestListingViewModel> GetAllManagerRequests(string managerId)
        {
            var result = _context.VacationRequests.Where(x => x.ReviewerId == managerId && x.StartDate.Day >= DateTime.UtcNow.Day && x.StartDate.Month >= DateTime.UtcNow.Month
            && x.StartDate.Year >= DateTime.UtcNow.Year && x.StatusId != (int)VacationRequestStatus.Approved)
                .Select(x => new VacationRequestListingViewModel
                {
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    EmpoyeeName = x.Requestor.FirstName + " " + x.Requestor.LastName,
                    Status = x.Status.Status,
                }).ToList();

            return result;
        }

        private int ValidateVacation(DateTime startDate, DateTime endDate, int countOfRequestedDays, int remainingVacationDays, string userId)
        {
            var currentYear = DateTime.UtcNow.Year;
            var currentMonth = DateTime.UtcNow.Month;
            var currentDay = DateTime.UtcNow.Day;

            if (startDate.Year < currentYear || startDate.Month < currentMonth || startDate.Day < currentDay)
            {
                return -1;
            }

            if (remainingVacationDays < countOfRequestedDays)
            {
                return -1;
            }

            var sameUserVacation = _context.VacationRequests.Where(x => x.RequestorId == userId && x.StatusId != (int)VacationRequestStatus.Rejected && x.StartDate == startDate
            && x.EndDate == endDate).ToList();

            if (sameUserVacation.Count != 0)
            {
                return -1;
            }

            return 0;
        }
    }
}
