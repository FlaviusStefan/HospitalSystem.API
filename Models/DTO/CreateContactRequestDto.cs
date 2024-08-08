using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.API.Models.DTO
{
    public class CreateContactRequestDto
    {
        [Required(ErrorMessage = "Mobile no. is required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
