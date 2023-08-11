using VacationsManagement.Data;
using VacationsManagement.Data.Models;
using VacationsManagement.Enumerations;
using VacationsManagement.Models.VacationRequests;
using VacationsManagement.Services.Users;

namespace VacationsManagement.Services.VacationRequests
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

            if (remainingVacationDays == 0 || managerId == null)
            {
                return -1;
            }

            var validationResult = ValidateVacation(startDate, endDate, countOfRequestedDays, remainingVacationDays, userId);

            if (validationResult == -1)
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

            return 0;
        }

        public IEnumerable<VacationRequestListingViewModel> GetAllManagerRequests(string managerId)
        {
            var result = _context.VacationRequests.Where(x => x.ReviewerId == managerId && x.StartDate.Day >= DateTime.UtcNow.Day && x.StartDate.Month >= DateTime.UtcNow.Month
            && x.StartDate.Year >= DateTime.UtcNow.Year && x.StatusId == (int)VacationRequestStatus.Pending)
                .Select(x => new VacationRequestListingViewModel
                {
                    Id = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    EmpoyeeName = x.Requestor.FirstName + " " + x.Requestor.LastName,
                    Status = x.Status.Status,
                    Vacationdays = x.DaysRequested
                }).ToList();

            return result;
        }

        public int ApproveVacationRequest(int requestId)
        {
            var request = _context.VacationRequests.FirstOrDefault(x => x.Id == requestId);

            if (request == null)
            {
                return -1;
            }

            request.StatusId = (int)VacationRequestStatus.Approved;

            _context.SaveChanges();

            _userService.UpdateVacationDaysByUserId(request.RequestorId, request.DaysRequested);

            return request.Id;
        }

        public int RejectVacationRequest(int requestId, string rejectReason)
        {
            var request = _context.VacationRequests.FirstOrDefault(x => x.Id == requestId);

            if (request == null)
            {
                return -1;
            }

            request.StatusId = (int)VacationRequestStatus.Rejected;
            request.RejectReason = rejectReason;

            _context.SaveChanges();

            return request.Id;
        }

        public IEnumerable<VacationRequestListingViewModel> GetMyRequests(string userId)
        {
            return _context.VacationRequests.Where(x => x.RequestorId == userId)
                .Select(x => new VacationRequestListingViewModel
                {
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status.Status,
                    RejectReason = x.RejectReason,
                    Vacationdays = x.DaysRequested
                }).ToList();
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

            //In a real application, each day from the request needs to be checked to see if it is included in any other vacation request. 
            //This could happen if the requests are separated to single days, one request will contains only one day. 
            //Also it could be added validation for holidays

            return 0;
        }
    }
}
