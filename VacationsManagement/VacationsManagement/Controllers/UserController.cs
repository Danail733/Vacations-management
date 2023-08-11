using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationsManagement.Infrastructure;
using VacationsManagement.Services.Users;

namespace VacationsManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        public IActionResult GetInfo()
        {
            var userId = User.Id();

            var userInfo = _userService.GetUserInfo(userId);

            return View(userInfo);
        }
    }
}
