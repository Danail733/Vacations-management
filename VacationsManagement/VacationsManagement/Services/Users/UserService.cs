using Microsoft.EntityFrameworkCore;
using VacationsManagement.Data;
using VacationsManagement.Models.Users;

namespace VacationsManagement.Services.Users
{
    public class UserService : IUserService
    {
        private readonly VacationManagementDbContext _context;

        public UserService(VacationManagementDbContext context)
        {
            _context = context;
        }

        public int GetVacationDaysByUserId(string userId)
        {
            return _context.Employees.FirstOrDefault(x => x.Id == userId).VacationDays;
        }

        public string GetManagerIdByUserId(string userId)
        {
            return _context.Employees.FirstOrDefault(x => x.Id == userId).ManagerId;
        }

        public int UpdateVacationDaysByUserId(string userId, int daysToSubtract)
        {
            var user = _context.Employees.FirstOrDefault(x => x.Id == userId);

            if(user == null)
            {
                return -1;
            }

            user.VacationDays = user.VacationDays - daysToSubtract;

            _context.SaveChanges();

            return user.VacationDays;
        }

        public UserInfoViewModel GetUserInfo(string userId)
        {
            var user = _context.Employees.Include(x => x.Manager)
                .FirstOrDefault(x => x.Id == userId);

            var result = new UserInfoViewModel
            {
                Email = user.Email,
                ManagerName = user.Manager.FirstName + " " + user.Manager.LastName,
                VacationDays = user.VacationDays
            };

            return result;
        }

    }
}
