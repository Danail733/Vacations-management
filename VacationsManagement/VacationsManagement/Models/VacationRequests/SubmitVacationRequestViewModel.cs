using System.ComponentModel.DataAnnotations;
using VacationsManagement.Utils.CustomAttributes;

namespace VacationsManagement.Models.VacationRequests
{
    public class SubmitVacationRequestViewModel
    {
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate  { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DateGreaterThan("StartDate", ErrorMessage = "End Date must be greater than Start Date.")]
        public DateTime EndDate { get; set; }
    }
}
