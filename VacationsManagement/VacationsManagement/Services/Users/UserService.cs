using VacationsManagement.Data;

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
            return _context.Employees.FirstOrDefault(x => x.Id == userId).DefaultCountOfVacationDays;
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

            user.DefaultCountOfVacationDays = user.DefaultCountOfVacationDays - daysToSubtract;

            _context.SaveChanges();

            return user.DefaultCountOfVacationDays;
        }

    }
}
