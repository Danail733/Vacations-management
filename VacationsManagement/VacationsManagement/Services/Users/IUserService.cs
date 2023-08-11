using VacationsManagement.Models.Users;

namespace VacationsManagement.Services.Users
{
    public interface IUserService
    {
        public int GetVacationDaysByUserId(string userId);

        public string GetManagerIdByUserId(string userId);

        public int UpdateVacationDaysByUserId(string userId, int daysToSubtract);

        public UserInfoViewModel GetUserInfo(string userId);
    }
}
