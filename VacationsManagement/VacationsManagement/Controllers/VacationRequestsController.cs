using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationsManagement.Infrastructure;
using VacationsManagement.Models.VacationRequests;
using VacationsManagement.Services.VacationRequests;

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

            return RedirectToAction("GetMyVacationRequests");
        }

        [HttpGet]
        [Authorize(Roles = WebConstants.managerRoleName)]
        public IActionResult GetAllManagerRequests()
        {
            var managerId = User.Id();
            var requests = _vacationRequestsService.GetAllManagerRequests(managerId);

            return View(requests);
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.managerRoleName)]
        public IActionResult ApproveVacationRequest(int requestId)
        {
            var result =_vacationRequestsService.ApproveVacationRequest(requestId);

            if(result == -1)
            {
                return BadRequest();
            }

            return RedirectToAction("GetAllManagerRequests");
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.managerRoleName)]
        public IActionResult RejectVacationRequest(int requestId, string rejectReason)
        {
            var result = _vacationRequestsService.RejectVacationRequest(requestId, rejectReason);

            if (result == -1)
            {
                return BadRequest();
            }

            return RedirectToAction("GetAllManagerRequests");
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetMyVacationRequests()
        {
            var userId = User.Id();
            var requests = _vacationRequestsService.GetMyRequests(userId);

            return View(requests);
        }
    }
}
