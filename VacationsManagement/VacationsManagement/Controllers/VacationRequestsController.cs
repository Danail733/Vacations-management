using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationsManagement.Infrastructure;
using VacationsManagement.Models.VacationRequests;
using VacationsManagement.Services;

namespace VacationsManagement.Controllers
{
    public class VacationRequestsController : Controller
    {
        private readonly IVacationRequestsService _vacationRequestsService;

        public VacationRequestsController(IVacationRequestsService vacationRequestsService)
        {
            _vacationRequestsService = vacationRequestsService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult SubmitVacationRequest()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult SubmitVacationRequest(SubmitVacationRequestViewModel inputModel)
        {
            var userId = User.Id();

            if(!ModelState.IsValid)
            {
                return View(inputModel);
            }

           var result =  _vacationRequestsService.SubmitVacationRequest(inputModel.StartDate, inputModel.EndDate, userId);

            if(result == -1)
            {
                return BadRequest();
            }

            return RedirectToAction("SubmitVacationRequest");
        }

        [HttpGet]
        [Authorize(Roles = WebConstanst.managerRoleName)]
        public IActionResult GetAllManagerRequests()
        {
            var managerId = User.Id();
           var requests = _vacationRequestsService.GetAllManagerRequests(managerId);

            return View(requests);
        }
    }
}
